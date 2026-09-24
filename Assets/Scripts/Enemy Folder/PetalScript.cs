using UnityEngine;

public class PetalScript : EnemyScript
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetHealthBar();
    }

    private void Update()
    {
        healthBarCanvas.transform.position = gameObject.transform.position;
    }

    public override void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        UpdateHealthBar();
        if (isCrit) { health -= damage * critMultiplier; }
        else { health -= damage; }

        if (health <= 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
            GameManager.instance.targetCount--;
            EventHandeler.onPetalDeath?.Invoke();
        }
    }

}
