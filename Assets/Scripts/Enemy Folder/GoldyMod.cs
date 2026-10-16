using UnityEngine;

public class GoldyMod : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<EnemyScript>().scoreValue *= 2; // double the score value of the enemy
    }
}
