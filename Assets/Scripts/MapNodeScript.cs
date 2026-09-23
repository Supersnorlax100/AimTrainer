using UnityEngine;

public class MapNodeScript : MonoBehaviour
{
    public int roomType;
    public int roomNum;

    public void GoToRoom()
    {
        MapManager.instance?.GoToRoom(roomNum);
    }
}
