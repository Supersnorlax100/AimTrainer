using UnityEngine;

public class EventHandeler : MonoBehaviour
{
    public delegate void OnTargetDeath();
    public static OnTargetDeath onTargetDeath;

}
