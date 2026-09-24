using UnityEngine;
using UnityEngine.UI;

public class MapNodeScript : MonoBehaviour
{
    public int roomType;
    public Vector2 roomNum;

    public void Start()
    {
        GetComponent<Image>().sprite = MapManager.instance.GetRoomSprite(roomNum);
    }
    public void GoToRoom()
    {
        MapManager.instance?.GoToRoom(roomNum);
    }
}
