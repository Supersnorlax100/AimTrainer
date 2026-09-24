using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public TargetType targetType;

    public float health;
    public float scoreValue;
    public GameObject healthBarCanvas;
    private Slider healthBar;
    public bool canGetHit = true;


    public virtual void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        if (canGetHit)
        {
            UpdateHealthBar();
            if (isCrit) { health -= damage * critMultiplier; }
            else { health -= damage; }

            if (health <= 0)
            {
                Destroy(gameObject.transform.parent.gameObject);
                GameManager.instance.targetCount--;
                EventHandeler.onEnemyDeath?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateHealthBar()
    {
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.value = health;
    }

    public void SetHealthBar()
    {
        healthBar = healthBarCanvas.GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);
    }
}
