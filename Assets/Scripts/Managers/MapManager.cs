using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        MakeMap();
        foreach (GameObject mapNode in nodeMap.Values)
        {
            Debug.Log(mapNode.name);
        }
    }

    public void GoToRoom(Vector2 room)
    {
        Debug.Log("go to room: " + room);
        Debug.Log(roomMap[room]);
        SceneManager.LoadScene((int)roomMap[room]);
    }

    private void MakeMap()
    {

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
        //string roomName = roomMap[roomPosition].ToString().ToLower();
        //Sprite roomSprite = roomSprites.Find(sprite => sprite.name.ToLower() == roomName);
        return roomSprite;
    }

    public void NodeAvailability()
    {
        //#TODO: Determine which buttons are available
    }
}
