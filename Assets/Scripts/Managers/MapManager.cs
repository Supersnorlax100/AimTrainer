using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

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
        Debug.Log("go to room: " +  room);
    }
}
