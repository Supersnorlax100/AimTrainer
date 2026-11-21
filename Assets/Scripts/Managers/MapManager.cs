using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RoomType
{
    PILOT,
    HOME,
    COMBAT,
    SHOP,
    EVENT,
    MINIBOSS,
    BOSS
}
public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public bool canLeaveRoom; // Can go to new room

    public GameObject map;

    // Room Types
    // 0 - Home
    // 1 - Combat
    // 2 - Shop
    // 3 - Event
    // 4 - MiniBoss
    // 5 - Boss
    private Dictionary<Vector2, RoomType> roomMap = new Dictionary<Vector2, RoomType>();
    private Dictionary<Vector2, GameObject> nodeMap = new Dictionary<Vector2, GameObject>();
    [SerializeField] private int floorsNumber;
    [SerializeField] private int RoomsPerFloor;
    public Vector2 currentRoom = new Vector2(1,-1);
    public RoomType currentRoomType;
    public List<Vector2> pastRooms = new List<Vector2>(1);
    public int floorsCleared;  
    public int currnetLvl;

    public Sprite[] sprites;
    [SerializeField] GameObject mapNode;
    public GameObject[] availableNodes = new GameObject[3];
    [SerializeField] GameObject mapNodeContainer;
    [SerializeField] int nodeSpacing;

    // Random Room Type
    Dictionary<RoomType, int> roomTypeOdds = new Dictionary<RoomType, int>()
    {
        { RoomType.COMBAT, 65},
        { RoomType.EVENT, 35},
        { RoomType.SHOP, 0},
        { RoomType.MINIBOSS, 0}
    };

    [SerializeField] int specalRoomOdds = 5; // aka miniBoss and shop
    [SerializeField] int shopOdds = 70;
    [SerializeField] int eventOdds = 25;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        EventHandeler.levelClear += LevelClear;
        EventHandeler.restart += Restart;
    }

    void OnDisable()
    {
        EventHandeler.levelClear -= LevelClear;
        EventHandeler.restart -= Restart;
    }

    private void Start()
    {
        pastRooms.Add(currentRoom);
        MakeMap();
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            EventHandeler.exitRoom?.Invoke();
        }
        if (Input.GetKeyDown("r"))
        {
            NewMap();
            currentRoom = new Vector2(1,-1);
            pastRooms.Add(currentRoom);
            NodeAvailability();
        }
        #endif

        if (GameManager.instance.forceLock || currentRoomType == RoomType.HOME) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            MapVisibility(!map.activeSelf);
        }
    }

    public void Restart()
    {
        NewMap();
        floorsCleared = 0;
        currnetLvl = 0;
        canLeaveRoom = true;
        _ = RestartGoToRoom();
    }

    // Just a copy of the standard function but without event call, pause, etc...
    public async Task RestartGoToRoom()
    {
        Vector2 room = new Vector2(1,-1);
        GameManager.instance.isSceneChanging = true;
        await SceneManager.LoadSceneAsync((int)roomMap[room]);
        GameManager.instance.isSceneChanging = false;
        currentRoom = room;
        currentRoomType = GetRoomType(room);
        pastRooms.Clear();
        pastRooms.Add(currentRoom);
        NodeAvailability();
        map.SetActive(false);
        // GameManager.instance.Pause(true);
    }

    public void LevelClear()
    {
        currnetLvl++;
        pastRooms.Clear();
        NewMap();
        currentRoom = new Vector2(1,-1);
        pastRooms.Add(currentRoom);
        NodeAvailability();
    }

    public void ExitRoom() // Called from Game manager from leave room button on room completion menu
    {
        canLeaveRoom = true;
        MapVisibility(true);
        floorsCleared++;

        if (currentRoomType == RoomType.BOSS)
        {
            EventHandeler.levelClear?.Invoke();
        }
    }

    public async Task GoToRoom(Vector2 room)
    {
        GameManager.instance.isSceneChanging = true;
        // Debug.Log("go to room");
        await SceneManager.LoadSceneAsync((int)roomMap[room]);
        // Debug.Log("loaded");
        GameManager.instance.isSceneChanging = false;
        currentRoom = room;
        currentRoomType = GetRoomType(room);
        pastRooms.Add(room);
        // Debug.Log("did past rooms");
        NodeAvailability();
        // Debug.Log("node availability");
        MapVisibility(!map.activeSelf);
        // Debug.Log("map visibility");
        canLeaveRoom = false;
        // Debug.Log("can leave");
        EventHandeler.enterRoom?.Invoke();
        // Debug.Log("event");
        // Debug.Log("go to room call: ");
        GameManager.instance.Pause(false);
        // Debug.Log("can leave room: " + canLeaveRoom);
    }

    private void MakeMap()
    {
        // Floor Dict
        for(int floor = 0; floor < floorsNumber; floor++)
        {
            for (int room = 0; room < RoomsPerFloor; room++)
            {
                Vector2 roomPosition = new Vector2(room, floor);
                roomMap.Add(roomPosition, GenerateRoomType(roomPosition));
                GenerateMapNode(roomPosition);
            }
        }
        // Home Node
        Vector2 _roomPosition = new Vector2(1, -1);
        RoomType roomType = RoomType.HOME;
        roomMap.Add(_roomPosition, roomType);
        GenerateMapNode(_roomPosition);
        // Boss Node
        _roomPosition = new Vector2(1, floorsNumber);
        roomType = RoomType.BOSS;
        roomMap.Add(_roomPosition, roomType);
        GenerateMapNode(_roomPosition);  
        
        NodeAvailability();
    }

    void GenerateMapNode(Vector2 roomPosition)
    {
        Vector3 mapNodePos = new Vector3(roomPosition.x * nodeSpacing - nodeSpacing, roomPosition.y * nodeSpacing - nodeSpacing*2 - 100, 0);
        GameObject _mapNode = Instantiate(mapNode, mapNodeContainer.transform);
        _mapNode.transform.localPosition = mapNodePos;
        _mapNode.GetComponent<MapNodeScript>().roomNum = roomPosition;
        nodeMap.Add(roomPosition, _mapNode);
    }

    RoomType GenerateRoomType(Vector2 roomPos)
    {
        if (roomPos.y != 0)
        {
            int randNum1 = Random.Range(0, 101);
            if (randNum1 <= specalRoomOdds)
            {
                specalRoomOdds -= 3;
                int randNum2 = Random.Range(0, 101);
                if (randNum2 <= shopOdds)
                {
                    shopOdds -= 5;
                    return RoomType.SHOP;
                }
                else
                {
                    shopOdds += 5;
                    return RoomType.MINIBOSS;
                }
            }
            else
            {
                specalRoomOdds += 1;
                int randNum3 = Random.Range(0, 101);
                if (randNum3 <= eventOdds)
                {
                    eventOdds -= 5;
                    // return RoomType.EVENT;
                    return RoomType.COMBAT;
                }
                else
                {
                    eventOdds += 1;
                    return RoomType.COMBAT;
                }
            }
        }

        else
        {
            specalRoomOdds += 1;
            int randNum3 = Random.Range(0, 101);
            if (randNum3 <= eventOdds)
            {
                eventOdds -= 5;
                // return RoomType.EVENT;
                return RoomType.COMBAT;
            }
            else
            {
                eventOdds += 1;
                return RoomType.COMBAT;
            }
        }
    } 

    public Sprite GetRoomSprite(Vector2 roomPosition)
    {
        Sprite roomSprite = sprites[(int)roomMap[roomPosition]-1]; //Because pilot room type
        return roomSprite;
    }

    public void NodeAvailability()
    {
        // Clears all nodes
        foreach (Vector2 roomPos in nodeMap.Keys)
        {
            nodeMap[roomPos].GetComponent<Image>().color = Color.black;
            nodeMap[roomPos].GetComponent<Button>().enabled = false;
            foreach (Vector2 pastPos in pastRooms)
            {
                if (roomPos == pastPos)
                {
                    nodeMap[roomPos].GetComponent<Image>().color = Color.gray;
                }
            }
        }
        
        // Checks for available nodes
        // Left
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x - 1, currentRoom.y + 1)))
        {
            availableNodes[0] = nodeMap[new Vector2(currentRoom.x - 1, currentRoom.y + 1)];
        }
        else {availableNodes[0] = null;}
        // Middle
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x, currentRoom.y + 1)))
        {
            availableNodes[1] = nodeMap[new Vector2(currentRoom.x, currentRoom.y + 1)];
        }
        else {availableNodes[1] = null;}
        // Right
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x + 1, currentRoom.y + 1)))
        {
            availableNodes[2] = nodeMap[new Vector2(currentRoom.x + 1, currentRoom.y + 1)];
        }
        else {availableNodes[2] = null;}
        foreach (GameObject node in availableNodes)
        {
            if (node == null) continue; // Skip null
    
            node.GetComponent<Button>().enabled = canLeaveRoom;
            node.GetComponent<Image>().color = Color.white;
        }
    }

    void NewMap()
    {
        roomMap.Clear();
        pastRooms.Clear();
        foreach (GameObject node in nodeMap.Values)
        {
            Destroy(node);
        }
        nodeMap.Clear();
        MakeMap();
        MapVisibility(true);
    }
    
    public bool IsUIRoom()
    {
        if (currentRoomType == RoomType.SHOP || currentRoomType == RoomType.EVENT || currentRoomType == RoomType.HOME) return true;
        else return false;
    }

    public void MapVisibility(bool isVisible)
    {
        if (UiManager.instance && ! IsUIRoom())
        {
            UiManager.instance?.CombatUI(!isVisible);
        }
        NodeAvailability();
        if (isVisible) AudioManager.instance.UiOpenSound();
        else AudioManager.instance.UiCloseSound();
        map.SetActive(isVisible);
        GameManager.instance.Pause(isVisible);
    }

    public RoomType GetRoomType(Vector2 roomPos)
    {
        return roomMap[roomPos];
    }

    // Game start
    public void OpenMap()
    {
        MapVisibility(true);
    }
}