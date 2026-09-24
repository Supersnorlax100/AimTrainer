using System.Collections.Generic;
using UnityEngine;

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
    private Dictionary<Vector2, RoomType> roomMap = new Dictionary<Vector2, RoomType>();
    [SerializeField] private List<Sprite> roomSprites = new List<Sprite>();
    [SerializeField] private int floorsNumber;
    [SerializeField] private int RoomsPerFloor;



    public void Start()
    {
        MakeMap();
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
