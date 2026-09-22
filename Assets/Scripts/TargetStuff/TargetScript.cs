using UnityEngine;
using UnityEngine.UI;
public enum TargetType
{
    CLICKING,
    SWITCHING,
    TRACKING
}

public class TargetScript : MonoBehaviour
{
    public TargetType targetType;

    public float health;
    public float scoreValue;
    public GameObject healthBarCanvas;
    private Slider healthBar;
    public bool canGetHit = true;
    public bool isEnemy = false;

    private void Start()
    {
        SetHealthBar();
    }

    private void Update()
    {
        healthBarCanvas.transform.position = transform.position;
    }

    public void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        if (canGetHit)
        {
            UpdateHealthBar();
            if (targetType == TargetType.CLICKING || targetType == TargetType.SWITCHING)
            {
                if (isCrit) { health -= damage * critMultiplier; }
                else { health -= damage; }

                if (health <= 0)
                {
                    if (isEnemy)
                    {
                        Destroy(gameObject.transform.parent.gameObject);
                        GameManager.instance.targetCount--;
                        EventHandeler.onEnemyDeath?.Invoke();
                        return;
                    }
                    Destroy(gameObject.transform.parent.gameObject);
                    GameManager.instance.targetCount--;
                    EventHandeler.onTargetDeath?.Invoke();
                }
            }
            else
            {
                EventHandeler.onTargetDeath?.Invoke();
            }
        }
        else
        {
            return;
        }
    }

    void UpdateHealthBar()
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
