using UnityEngine;
using UnityEngine.UI;

public class TargetScript : MonoBehaviour
{
    public float health;
    public float scoreValue;
    [SerializeField] private GameObject healthBarCanvas;
    private Slider healthBar;
    public float force;
    [SerializeField] float persentageOfForce;
    private void Awake()
    {
        healthBar = healthBarCanvas.GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        healthBarCanvas.transform.position = transform.position;
        
    }


    //public void Addforce()
    //{
    //    gameObject.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Rigidbody>(). * force / persentageOfForce, ForceMode.Impulse);
    //}


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
