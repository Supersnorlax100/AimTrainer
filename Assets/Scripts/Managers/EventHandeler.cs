using UnityEngine;

public class EventHandeler : MonoBehaviour
{
    public delegate void OnTargetDeath();
    public static OnTargetDeath onTargetDeath;

    public delegate void OnPlayerDeath();
    public static OnPlayerDeath onPlayerDeath;

    public delegate void OnEnemyDeath();
    public static OnEnemyDeath onEnemyDeath;

    public delegate void OnPetalDeath();
    public static OnPetalDeath onPetalDeath;
}
