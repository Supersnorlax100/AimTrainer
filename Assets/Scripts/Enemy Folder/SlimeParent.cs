using UnityEngine;

public class SlimeParent : MonoBehaviour
{
    private void Start()
    {
        EventHandeler.onSubEnemyDeath += CheckChildren;
        EventHandeler.onEnemyDeath += CheckChildren;
    }
    private void OnDisable()
    {
        EventHandeler.onEnemyDeath -= CheckChildren;
        EventHandeler.onSubEnemyDeath -= CheckChildren;
    }

    public void CheckChildren()
    {
        if (GetComponentsInChildren<Transform>().Length == 1)
        {
            Destroy(gameObject);
            return;
        }
    }
}
