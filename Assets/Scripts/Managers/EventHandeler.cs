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
    public static OnPetalDeath onSubEnemyDeath;

    public delegate void Purchase();
    public static Purchase purchase;

    public delegate void UpdateUI();
    public static UpdateUI updateUI;

    public delegate void ExitRoom();
    public static ExitRoom exitRoom;
}
