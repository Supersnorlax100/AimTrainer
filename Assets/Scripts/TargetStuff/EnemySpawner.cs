using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour

{
    [SerializeField] GameObject spawnArea;
    [SerializeField] GameObject[] enemies;
    [SerializeField] string[] enemynames;
    Dictionary<string, GameObject> enemyDictionary = new Dictionary<string, GameObject>();
    GameObject enemy;
    [SerializeField] GameObject enemyParent;
    [SerializeField] int initialEnemyCount;
    [SerializeField] int attemptsToSpawn = 1000;

    public float enemySpace;

    private float XspawnAreaScale;
    private float YspawnAreaScale;

    public bool areEnemysMoving;
    public float enemySpeed = 1;
    public float enemySpeedVariability = 0;

    private void Awake()
    {
        EventHandeler.onEnemyDeath += Spawn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XspawnAreaScale = spawnArea.transform.localScale.x;
        YspawnAreaScale = spawnArea.transform.localScale.y;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyDictionary.Add(enemynames[i], enemies[i]);
        }
        enemy = enemyDictionary["flower"];


        for (int i = 0; i < initialEnemyCount; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < attemptsToSpawn; i++)
        {
            // The + YspawnAreaScale/10 is because the target spawns a little too low than what it should so I added an offset
            // float targetX = spawnArea.transform.position.x + Random.Range(-(XspawnAreaScale / 2), XspawnAreaScale / 2);
            // float targetY = spawnArea.transform.position.y + Random.Range(-(YspawnAreaScale / 2), YspawnAreaScale / 2);
            float enemyX = spawnArea.transform.position.x + Random.Range(-(XspawnAreaScale / 2) + XspawnAreaScale / 25, XspawnAreaScale / 2 - XspawnAreaScale / 25);
            float enemyY = spawnArea.transform.position.y + Random.Range(-(YspawnAreaScale / 2) + YspawnAreaScale / 10, YspawnAreaScale / 2 + YspawnAreaScale / 25);
            float enemyZ = spawnArea.transform.position.z + 0.3f; // just added offset
            Vector3 enemyPos = new Vector3(enemyX, enemyY, enemyZ);

            if (Physics.OverlapSphere(enemyPos, enemy.GetComponentInChildren<SphereCollider>().radius + enemySpace).Length == 1 || i == attemptsToSpawn - 1)
            {
                GameObject _enemy = Instantiate(enemy, enemyPos, Quaternion.identity, enemyParent.transform);
                _enemy = _enemy.transform.GetChild(0).gameObject;
                if (areEnemysMoving)
                {
                    Vector3 _enemyDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
                    float _enemySpeed = enemySpeed + Random.Range(-(enemySpeedVariability), enemySpeedVariability);
                    _enemy.GetComponent<Rigidbody>().AddForce(_enemyDirection.normalized * _enemySpeed, ForceMode.Impulse);
                }
                return;
            }
        }
        Debug.LogError("Could not spawn enemy.");

    }

    public void RespawnAll()
    {
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Spawn();
        }
    }

    public void Clearenemys()
    {
        TargetScript[] allenemys = GetComponentsInChildren<TargetScript>();
        foreach (TargetScript _enemy in allenemys)
        {
            Destroy(_enemy.gameObject.transform.parent.gameObject);
        }
    }

}

