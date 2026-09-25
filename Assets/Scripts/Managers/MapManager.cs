using System.Collections.Generic;
using System.Linq;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        pastRooms.Add(currentRoom);
        MakeMap();
        foreach (GameObject mapNode in nodeMap.Values)
        {
            Debug.Log(mapNode.name);
        }
        NodeAvailability();
    }

    public void GoToRoom(Vector2 room)
    {
        SceneManager.LoadScene((int)roomMap[room]);
        currentRoom = room;
        pastRooms.Add(room);
        NodeAvailability();
    }

    private void MakeMap()
    {
        // Floor Dict
        for(int floor = 0; floor < floorsNumber; floor++)
        {
            for (int room = 0; room < RoomsPerFloor; room++)
            {
                Vector2 roomPosition = new Vector2(room, floor);
                // The 1 and -1 are to ignore the home and boss sprites
                int randomRoomTypeIndex = Random.Range(1, System.Enum.GetValues(typeof(RoomType)).Length -1);
                RoomType randomRoomType = (RoomType)randomRoomTypeIndex; 
                roomMap.Add(roomPosition, randomRoomType);
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
    }

    void GenerateMapNode(Vector2 roomPosition)
    {
        Vector3 mapNodePos = new Vector3(roomPosition.x * 150 - 150, roomPosition.y * 150 - 300, 0);
        GameObject _mapNode = Instantiate(mapNode, Vector3.zero, Quaternion.identity, transform);
        _mapNode.transform.localPosition = mapNodePos;
        _mapNode.GetComponent<MapNodeScript>().roomNum = roomPosition;
        nodeMap.Add(roomPosition, _mapNode);
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
                Debug.Log("pastPos: " + pastPos);
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
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x, currentRoom.y + 1)))
        {
            nodeMap[new Vector2(currentRoom.x, currentRoom.y + 1)].GetComponent<Button>().enabled = true;
            nodeMap[new Vector2(currentRoom.x, currentRoom.y + 1)].GetComponent<Image>().color = Color.white;
        }
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x - 1, currentRoom.y + 1)))
        {
            nodeMap[new Vector2(currentRoom.x - 1, currentRoom.y + 1)].GetComponent<Button>().enabled = true;
            nodeMap[new Vector2(currentRoom.x - 1, currentRoom.y + 1)].GetComponent<Image>().color = Color.white;
        }
        if (nodeMap.ContainsKey(new Vector2(currentRoom.x + 1, currentRoom.y + 1)))
        {
            nodeMap[new Vector2(currentRoom.x + 1, currentRoom.y + 1)].GetComponent<Button>().enabled = true;
            nodeMap[new Vector2(currentRoom.x + 1, currentRoom.y + 1)].GetComponent<Image>().color = Color.white;
        }
    }
}