using UnityEngine;

public class CountDownScript : MonoBehaviour
{
    // TEMP SCRIPT
    public void ExitRoom()
    {
        EventHandeler.exitRoom?.Invoke();
    }
}
