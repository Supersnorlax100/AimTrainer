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

    private void Start()
    {
        SetHealthBar();
    }

    private void Update()
    {
        healthBarCanvas.transform.position = transform.position;
    }

    public virtual void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        if (canGetHit)
        {
            if (isCrit) { health -= damage * critMultiplier; }
            else { health -= damage; }
           
            UpdateHealthBar();
        
            if (health <= 0)
            {
                if (isEnemy)
                {
                    EventHandeler.onEnemyDeath?.Invoke();
                    Destroy(gameObject.transform.parent.gameObject);
                    //EventHandeler.onEnemyDeath?.Invoke();
                    return;
                }
                Destroy(gameObject.transform.parent.gameObject);
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
