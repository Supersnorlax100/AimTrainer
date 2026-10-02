using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    public TargetType targetType;

    public float health;
    public float scoreValue;
    public GameObject healthBarCanvas;
    private Slider healthBar;

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
        UpdateHealthBar();
        if (targetType == TargetType.CLICKING || targetType == TargetType.SWITCHING)
        {
            if (isCrit) { health -= damage * critMultiplier; }
            else { health -= damage; }

            if (health <= 0)
            {
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
