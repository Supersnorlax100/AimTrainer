using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int health;
    public int scoreValue;
    private Slider healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);
    }
    public void GetHit(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            EventHandeler.onTargetDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void CritHit()
    {

    }
    void UpdateHealthBar()
    {
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.value = health;
    }
}
