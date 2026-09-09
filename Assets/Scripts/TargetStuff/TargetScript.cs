using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    public float health;
    public int scoreValue;
    [SerializeField] private GameObject healthBarCanvas;
    private Slider healthBar;

    private void Awake()
    {
        healthBar = healthBarCanvas.GetComponentInChildren<Slider>();

        
    }

    private void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);


    }

    private void Update()
    {
        healthBarCanvas.transform.position = transform.position;
        
    }


    public void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        if (isCrit) { health -= damage * critMultiplier; }
        else { health -= damage; }
           
        UpdateHealthBar();
        
        if (health <= 0)
        {
            EventHandeler.onTargetDeath?.Invoke();
            Destroy(gameObject.transform.parent.gameObject);
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
}
