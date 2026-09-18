using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    public Collider[] targetCollisions;
    public Collider[] spawnableCollisions;

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
            // The + YtargetSpawnAreaScale/10 is because the target spawns a little too low than what it should so I added an offset
            // The sqrt is to out to if a square was rotated 45             
            float targetX = spawnArea.transform.position.x + Random.Range(-(XspawnAreaScale / 2) + XspawnAreaScale / 25, XspawnAreaScale / 2 - XspawnAreaScale / 25);
            float targetY = spawnArea.transform.position.y + Random.Range(-(YspawnAreaScale / 2), YspawnAreaScale / 2);
            float targetZ = enemy.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().radius + 0.1f;
            Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

            // Test a collider in given area
            targetCollisions = Physics.OverlapSphere(targetPos, enemy.GetComponentInChildren<SphereCollider>().radius + enemySpace);
            spawnableCollisions = Physics.OverlapSphere(targetPos, 0.1f);
            if (spawnableCollisions.Length == 1 && spawnableCollisions[0].gameObject.layer == 9 && targetCollisions.Length > 0 && (targetCollisions[0].gameObject.layer != 6 || targetCollisions[1].gameObject.layer != 6))
            {
                GameObject _target = Instantiate(enemy, targetPos, Quaternion.identity, enemyParent.transform);
                _target = _target.transform.GetChild(0).gameObject;
                if (areEnemysMoving)
                {
                    Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
                    float _targetSpeed = enemySpeed + Random.Range(-(enemySpeedVariability), enemySpeedVariability);
                    _target.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);

                }
                return;
            }
            if (i == attemptsToSpawn - 1)
            {
                Debug.Log("force");
                for (int j = 0; j < 1000000; j++)
                {
                    if (spawnableCollisions.Length >= 1 && spawnableCollisions[0].gameObject.layer == 9)
                    {
                        GameObject _target = Instantiate(enemy, targetPos, Quaternion.identity, enemyParent.transform);
                        _target = _target.transform.GetChild(0).gameObject;
                        if (areEnemysMoving)
                        {
                            Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
                            float _targetSpeed = enemySpeed + Random.Range(-(enemySpeedVariability), enemySpeedVariability);
                            _target.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);
                        }
                        return;
                    }
                }
                Debug.LogError("Could not spawn target.");
            }
        }

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

