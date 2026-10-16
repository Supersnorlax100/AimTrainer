using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SnorlaxMod : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    List<GameObject> nodes = new List<GameObject>();
    float maxHealth;
    bool HasHealed = false;

    public void Start()
    {
        maxHealth = GetComponent<EnemyScript>().health;
    }
    public void GetHit()
    {
        Debug.Log("SnorlaxMod: GetHit called");

        if (GetComponent<EnemyScript>().health <= maxHealth /2 && !HasHealed)
        {
            Debug.Log("Heal");
            HasHealed = true;
            healPhase();
            GetComponent<EnemyScript>().canGetHit = false;
        }
    }

    void healPhase()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0, 0, .25f);
        SpawnNodes();
        Invoke("Timer", 5f); // change this to for the amount of time the player gets to destroy the nodes before the enemy heals.
    }

    void Timer()
    {
        foreach (GameObject node in nodes)
        {
            if (node != null)
            {
                Destroy(node);
                GetComponent<EnemyScript>().health += 1; // heal amount
            }
        }
        GetComponent<EnemyScript>().canGetHit = true;
        Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
        float _targetSpeed = 3; // change this to the speed you want the enemy to move after the heal phase.
        GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);

    }

    public void SpawnNodes()
    {
        float degreasBetweenNodes = 360 / 7; // cannge this to the number of nodes you want to spawn
        GameObject parentNode = new GameObject("NodeParent");
        parentNode.transform.parent = transform;
        parentNode.transform.localPosition = Vector3.zero;
        for (int i = 7; i > 0; i--) // this number should be the same as the number of nodes you want to spawn
        {
            GameObject newNode = Instantiate(prefab, parentNode.transform);
            newNode.transform.GetChild(0).gameObject.AddComponent<SubEnemyScript>();
            parentNode.transform.localRotation = Quaternion.Euler(0, 0, degreasBetweenNodes * i);
            newNode.gameObject.transform.position = gameObject.transform.position + new Vector3(0,2,0);
            nodes.Add(newNode);

        }
    }
}
