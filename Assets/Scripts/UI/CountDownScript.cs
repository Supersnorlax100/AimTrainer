using UnityEngine;

public class CountDownScript : MonoBehaviour
{
    // TEMP SCRIPT
    public void ExitRoom()
    {
        Debug.Log("button exit");
        EventHandeler.exitRoom?.Invoke();
    }
}
