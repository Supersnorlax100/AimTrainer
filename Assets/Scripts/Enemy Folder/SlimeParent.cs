using UnityEngine;

public class SlimeParent : MonoBehaviour
{
    private void Start()
    {
        EventManager.onSubEnemyDeath += CheckChildren;
        EventManager.onEnemyDeath += CheckChildren;
    }
    private void OnDisable()
    {
        EventManager.onEnemyDeath -= CheckChildren;
        EventManager.onSubEnemyDeath -= CheckChildren;
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
