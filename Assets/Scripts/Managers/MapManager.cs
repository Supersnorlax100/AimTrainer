using System.Collections.Generic;
using UnityEngine;
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

    private Dictionary<Vector2, RoomType> roomMap = new Dictionary<Vector2, RoomType>();
    [SerializeField] private List<Sprite> roomSprites = new List<Sprite>();
    [SerializeField] private int floorsNumber;
    [SerializeField] private int RoomsPerFloor;

    public int[] rooms;
    public GameObject[] roomObjs;
    public Sprite[] sprites;

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
        for (int i = 0; i < rooms.Length; i++)
        {
            int j = Random.Range(0, 4);
            rooms[i] = j;
            roomObjs[i].GetComponent<MapNodeScript>().roomNum = i;
            roomObjs[i].GetComponent<MapNodeScript>().roomType = j;
            roomObjs[i].GetComponent<Image>().sprite = sprites[j];
        }
    }

    public void GoToRoom(int room)
    {
        Debug.Log("go to room: " + room);
        
    }

    private void MakeMap()
    {

        for(int floor = 0; floor < floorsNumber; floor++)
        {
            for (int room = 0; room < RoomsPerFloor; room++)
            {
                Vector2 roomPosition = new Vector2(room, floor);
                int randomRoomTypeIndex = Random.Range(0, System.Enum.GetValues(typeof(RoomType)).Length);
                RoomType randomRoomType = (RoomType)randomRoomTypeIndex; 
                roomMap.Add(roomPosition, randomRoomType);
            }
        }
    }

    public Sprite GetRoomSprite(Vector2 roomPosition)
    {
        string roomName = roomMap[roomPosition].ToString().ToLower();
        Sprite roomSprite = roomSprites.Find(sprite => sprite.name.ToLower() == roomName);
        return roomSprite;
    }


}
