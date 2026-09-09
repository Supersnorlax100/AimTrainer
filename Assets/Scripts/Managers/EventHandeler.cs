using UnityEngine;

public class EventHandeler : MonoBehaviour
{
    public delegate void OnTargetDeath();
    public static OnTargetDeath onTargetDeath;

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

}
