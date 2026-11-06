using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum RoomType
{
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
    [SerializeField] private int roomsPerFloor;
    public Vector2 currentRoom = new Vector2(1,-1);
    public RoomType currentRoomType;
    List<Vector2> pastRooms = new List<Vector2>(1);
    public int floorsCleared;  
    public int currnetLvl;

    public Sprite[] sprites;
    [SerializeField] GameObject mapNode;
    GameObject[] availableNodes = new GameObject[3];
    [SerializeField] GameObject mapNodeContainer;
    [SerializeField] int nodeSpacing;

    // Random Room Type
    Dictionary<RoomType, int> roomTypeNum = new Dictionary<RoomType, int>()
    {
        { RoomType.COMBAT, 9999},
        { RoomType.EVENT, 99999},
        { RoomType.SHOP, 2},
        { RoomType.MINIBOSS, 3}
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
        // SceneManager.sceneLoaded += OnSceneLoaded;

        EventHandeler.exitRoom += ExitRoom;
        EventHandeler.levelClear += LevelClear;
    }

    private void Start()
    {
        pastRooms.Add(currentRoom);
        MakeMap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            EventHandeler.exitRoom?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MapVisibility(!map.activeSelf);
        }
        if (Input.GetKeyDown("r"))
        {
            NewMap();
        }
        if (map.activeSelf)
        {
            GameManager.instance.Pause(true);
        }
    }

    public void LevelClear()
    {
        currnetLvl++;
        pastRooms.Clear();
        currentRoom = new Vector2(1,-1);
        pastRooms.Add(currentRoom);
        NewMap();
    }

    void ExitRoom()
    {
        canLeaveRoom = true;
        MapVisibility(true);
        floorsCleared++;
        // Debug.Log("floors cleared: " + floorsCleared);
    }

    public async Task GoToRoom(Vector2 room)
    {
        await SceneManager.LoadSceneAsync((int)roomMap[room]);
        currentRoom = room;
        currentRoomType = GetRoomType(room);
        pastRooms.Add(room);
        NodeAvailability();
        MapVisibility(!map.activeSelf);
        canLeaveRoom = false;
        EventHandeler.enterRoom?.Invoke();
        GameManager.instance.Pause(false);
    }

    private void MakeMap()
    {
        // Floor Dict
        for(int floor = 0; floor < floorsNumber; floor++)
        {
            for (int room = 0; room < roomsPerFloor; room++)
            {
                Vector2 roomPos = new Vector2(room, floor);
                roomMap.Add(roomPos, GenerateRoomType(roomPos));
                GenerateMapNode(roomPos);
            }
        }
        // Home Node
        Vector2 _roomPos = new Vector2(1, -1);
        RoomType roomType = RoomType.HOME;
        roomMap.Add(_roomPos, roomType);
        GenerateMapNode(_roomPos);
        // Boss Node
        _roomPos = new Vector2(1, floorsNumber);
        roomType = RoomType.BOSS;
        roomMap.Add(_roomPos, roomType);
        GenerateMapNode(_roomPos);  
        
        NodeAvailability();
    }

    void GenerateMapNode(Vector2 roomPos)
    {
        Vector3 mapNodePos = new Vector3(roomPos.x * nodeSpacing - nodeSpacing, roomPos.y * nodeSpacing - nodeSpacing*2 - 100, 0);
        GameObject _mapNode = Instantiate(mapNode, mapNodeContainer.transform);
        _mapNode.transform.localPosition = mapNodePos;
        _mapNode.GetComponent<MapNodeScript>().roomNum = roomPos;
        nodeMap.Add(roomPos, _mapNode);
    }

    List<Vector2> RoomTypeMap(RoomType roomType)
    {
        List<Vector2> madePositions = new List<Vector2>();

        if (roomType == RoomType.SHOP)
        {
            for (int i = 0; i < roomsPerFloor; i++)
            {
                madePositions.Add(new Vector2(i, floorsNumber));
            }
            // for (int i = 0; i < roomTypeNum[roomType]; i++)
            // {
                
            // }
        }

        if (roomType != RoomType.COMBAT || roomType != RoomType.EVENT)
        {
            for (int i = 0; i < roomTypeNum[roomType]; i++)
            {
                if (roomType == RoomType.SHOP)
                {
                    madePositions.Add(new Vector2(Random.Range(0, roomsPerFloor + 1), Random.Range(1, floorsNumber - 3)));
                }
            }
        }

        // for(int floor = 0; floor < floorsNumber; floor++)
        // {
        //     for (int room = 0; room < RoomsPerFloor; room++)
        //     {
        //         if (floor == 0) {break;}
        //         switch (roomType)
        //         {
        //             case RoomType.SHOP:
        //             {
        //                 foreach ()
        //                 break;
        //             }
        //             default: break;
        //         }
        //         // list.Add(new Vector2(room, floor));
        //     }
        // }

        return madePositions;
    }

    RoomType GenerateRoomType(Vector2 roomPos)
    {
        // if floor before boss
        if (roomPos.y == floorsNumber-1)
        {
            return RoomType.SHOP;   
        }

        return RoomType.HOME;
        // if (roomPos.y != 0)
        // {
        //     int randNum1 = Random.Range(0, 101);
        //     if (randNum1 <= specalRoomOdds)
        //     {
        //         specalRoomOdds -= 3;
        //         int randNum2 = Random.Range(0, 101);
        //         if (randNum2 <= shopOdds)
        //         {
        //             shopOdds -= 5;
        //             return RoomType.SHOP;

        //         }
        //         else
        //         {
        //             shopOdds += 5;
        //             return RoomType.MINIBOSS;

        //         }

        //     }
        //     else
        //     {
        //         specalRoomOdds += 1;
        //         int randNum3 = Random.Range(0, 101);
        //         if (randNum3 <= eventOdds)
        //         {
        //             eventOdds -= 5;
        //             return RoomType.EVENT;
        //         }
        //         else
        //         {
        //             eventOdds += 1;
        //             return RoomType.COMBAT;
        //         }
        //     }
        // }

        // else
        // {
        //     specalRoomOdds += 1;
        //     int randNum3 = Random.Range(0, 101);
        //     if (randNum3 <= eventOdds)
        //     {
        //         eventOdds -= 5;
        //         return RoomType.EVENT;
        //     }
        //     else
        //     {
        //         eventOdds += 1;
        //         return RoomType.COMBAT;
        //     }
        // }
    } 

    public Sprite GetRoomSprite(Vector2 roomPosition)
    {
        Sprite roomSprite = sprites[(int)roomMap[roomPosition]];
        return roomSprite;
    }

    public void NodeAvailability()
    {
        // Clears all nodes
        foreach (Vector2 roomPos in nodeMap.Keys)
        {
            nodeMap[roomPos].GetComponent<Button>().enabled = false;
            foreach (Vector2 pastPos in pastRooms)
            {
                if (roomPos == pastPos)
                {
                    nodeMap[roomPos].GetComponent<Image>().color = Color.white;
                    break;
                }
                else
                {
                    nodeMap[roomPos].GetComponent<Image>().color = Color.black;
                }
            }
        }

        // Checks for available nodes
        // Left
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x - 1, currentRoom.y + 1)))
        {
            availableNodes[0] = nodeMap[new Vector2(currentRoom.x - 1, currentRoom.y + 1)];
        }
        // Middle
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x, currentRoom.y + 1)))
        {
            availableNodes[1] = nodeMap[new Vector2(currentRoom.x, currentRoom.y + 1)];
        }
        // Right
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x + 1, currentRoom.y + 1)))
        {
            availableNodes[2] = nodeMap[new Vector2(currentRoom.x + 1, currentRoom.y + 1)];
        }
        foreach (GameObject node in availableNodes)
        {
            node.GetComponent<Image>().color = Color.white;
        }
    }

    void NewMap()
    {
        roomMap.Clear();
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

    void MapVisibility(bool isVisible)
    {
        if (UiManager.instance && ! IsUIRoom())
        {
            UiManager.instance?.CombatUI(!isVisible);
        }
        if (isVisible)
        {
            foreach (GameObject node in availableNodes)
            {
                node.GetComponent<Button>().enabled = canLeaveRoom;
            }
        }
        map.SetActive(isVisible);
        GameManager.instance.Pause(isVisible);
    }

    public RoomType GetRoomType(Vector2 roomPos)
    {
        return roomMap[roomPos];
    }

    public void OpenMap()
    {
        MapVisibility(true);
    }
}