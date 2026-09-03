using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int health;
    public int scoreValue;

    public void Start()
    {
        
    }
    public void GetHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EventHandeler.onTargetDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void CritHit()
    {

    }

    public void Die()
    {

    }
}
