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
    
    public bool canMove; // Can go to new room

    [SerializeField] GameObject map;

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

    Vector2 currentRoom = new Vector2(1,-1);
    List<Vector2> pastRooms = new List<Vector2>(1);

    public Sprite[] sprites;
    [SerializeField] GameObject mapNode;
    GameObject[] availableNodes = new GameObject[3];
    [SerializeField] GameObject mapNodeContainer;
    [SerializeField] int nodeSpacing;

    // Random Room Type
    public bool canMiniBossSpawn;
    public bool canShopSpawn;
    public bool canEventSpawn;
    public int miniBossOdds;
    public int shopOdds;
    public int eventOdds;

    // GameObject[] activeCanvases;

    public int curentFloor; //Temp Destroy me i am called in EnemySpawner

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
    }

    private void Start()
    {
        pastRooms.Add(currentRoom);
        MakeMap();
    }

    void Update()
    {
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

        // #TODO: move to happen on stage end
        foreach (GameObject node in availableNodes)
        {
            node.GetComponent<Button>().enabled = canMove;
        }
    }

    public async Task GoToRoom(Vector2 room)
    {
        await SceneManager.LoadSceneAsync((int)roomMap[room]);
        currentRoom = room;
        pastRooms.Add(room);
        NodeAvailability();
        MapVisibility(!map.activeSelf);
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
        if (roomPos.y >= Mathf.Floor(floorsNumber/2))
        {
            canMiniBossSpawn = true;
        }
        if (roomPos.y >= Mathf.Floor(floorsNumber/3))
        {
            canShopSpawn = true;
            canEventSpawn = true;
        }

        int randNum = Random.Range(0,101);
        if (randNum >= miniBossOdds && canMiniBossSpawn)
            return RoomType.MINIBOSS;
        else if (randNum >= shopOdds && canShopSpawn)
            return RoomType.SHOP;
        else if (randNum >= eventOdds && canEventSpawn)
            return RoomType.EVENT;
        else
            return RoomType.COMBAT;
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

        canMiniBossSpawn = false;
        canEventSpawn = false;
        canShopSpawn = false;

        MakeMap();
    }

    // Runs automatically whenever any scene finishes loading
    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     activeCanvases = GameObject.FindGameObjectsWithTag("Canvas");
    // }

    void MapVisibility(bool isVisible)
    {
        // foreach (GameObject canvas in activeCanvases)
        // {
        //     canvas.SetActive(!isVisible);
        // }
        if (UiManager.instance)
        {
            UiManager.instance?.CombatUI(!isVisible);
        }
        map.SetActive(isVisible);
        GameManager.instance.Pause(isVisible);
    }

    // for button
    public void OpenMap()
    {
        MapVisibility(true);
    }
}