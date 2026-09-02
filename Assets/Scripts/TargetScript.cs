using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int scoreValue;

    public void GetHit(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void CritHit()
    {

    }

    public void Die()
    {
        PlayerControler.instance.AddScore(scoreValue);
        Destroy(gameObject);
    }




}
