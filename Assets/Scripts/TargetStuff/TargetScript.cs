using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    public float health;
    public float scoreValue;
    public GameObject healthBarCanvas;
    private Slider healthBar;
    public bool canGetHit = true;
    public bool isEnemy = false;

    public enum TargetType
    {
        CLICKING,
        SWITCHING,
        TRACKING
    }
    public TargetType targetType;

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
                Debug.Log("click or switch");
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
                Debug.Log("track");
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
