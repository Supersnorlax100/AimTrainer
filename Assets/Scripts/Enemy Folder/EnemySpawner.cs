using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySpawner : MonoBehaviour
{



    [SerializeField] GameObject[] trakingEnemies;
    [SerializeField] GameObject[] switchingEnemies;
    [SerializeField] GameObject[] clikingEnemies;
    GameObject enemy;

    float randomHealth;


    [SerializeField] int attemptsToSpawn = 1000;
    public Collider[] targetCollisions;
    public Collider[] spawnableCollisions;
    public float enemySpace;

    [SerializeField] GameObject spawnArea;
    [SerializeField] GameObject enemyParent;
    private float XspawnAreaScale;
    private float YspawnAreaScale;

    [SerializeField] int initialEnemyCount;
    int numberOfSpawnedEnemys;
    [SerializeField] int maxEnemyCount;
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

        generateRandomHealth();
        PickEnemy();
        for (int i = 0; i < initialEnemyCount; i++)
        {
            Spawn();
        }
    }

    private void RandomEnemeyChoice(TargetType enemyType)
    {
        switch (enemyType)
        {
            case TargetType.CLICKING:
                enemy = clikingEnemies[Random.Range(0, clikingEnemies.Length)];
                break;
            case TargetType.SWITCHING:
                enemy = switchingEnemies[Random.Range(0, switchingEnemies.Length)];
                break;
            case TargetType.TRACKING:
                enemy = trakingEnemies[Random.Range(0, trakingEnemies.Length)];
                break;
            default:
                Debug.LogError("Error in RandomEnemeyChoice: Invalid enemy type.");
                break;
        }
    }



    public void PickEnemy()
    {
        switch(GameManager.instance.stageType)
        {
            case TargetType.CLICKING:
                enemy = clikingEnemies[Random.Range(0, clikingEnemies.Length)];
                break;
            case TargetType.SWITCHING:
                enemy = switchingEnemies[Random.Range(0, switchingEnemies.Length)];
                break;
            case TargetType.TRACKING:
                enemy = trakingEnemies[Random.Range(0, trakingEnemies.Length)];
                break;
            default:
                Debug.LogError("Error in PickEnemyPool: Invalid stage type.");
                break;
        }
    }
    private void generateRandomHealth()
    {
        float maxHealth = Mathf.Pow(1.3f, MapManager.instance.curentFloor) *5;
        randomHealth = Random.Range(1,maxHealth);
    }
    public Vector3 GenerateSpawnLocation()
    {
        // The + YtargetSpawnAreaScale/10 is because the target spawns a little too low than what it should so I added an offset
        // The sqrt is to out to if a square was rotated 45             
        float targetX = spawnArea.transform.position.x + Random.Range(-(XspawnAreaScale / 2) + XspawnAreaScale / 25, XspawnAreaScale / 2 - XspawnAreaScale / 25);
        float targetY = spawnArea.transform.position.y + Random.Range(-(YspawnAreaScale / 2), YspawnAreaScale / 2);
        float targetZ = spawnArea.transform.position.z + enemy.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().radius - .2f;
        Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

        // Test a collider in given area
        targetCollisions = Physics.OverlapSphere(targetPos, enemy.GetComponentInChildren<SphereCollider>().radius + enemySpace);
        spawnableCollisions = Physics.OverlapSphere(targetPos, 0.1f);

        #region Debugs
        //Debug.Log("atemted spawn pos: " + targetPos);
        //Debug.Log("length: " + targetCollisions.Length);
        //Debug.Log("target coll: ");
        //foreach (Collider coll in targetCollisions) { Debug.Log("targ coll: " + coll.name); }
        //Debug.Log("spawnable collisions length: " + spawnableCollisions.Length);
        //Debug.Log("spawnable collisions layer: " + spawnableCollisions[0].gameObject.layer);
        //Debug.Log("colliders: ");
        #endregion

        foreach (Collider coll in spawnableCollisions) { Debug.Log("spawn coll: " + coll.name); }

        if (spawnableCollisions.Length == 1 && spawnableCollisions[0].gameObject.layer == 9 && targetCollisions.Length > 0 && (targetCollisions[0].gameObject.layer != 6 || targetCollisions[1].gameObject.layer != 6))
        {
            return targetPos;
        }
        return Vector3.zero;
    }

    public void Spawn()
    {

        for (int i = 0; i < attemptsToSpawn; i++)
        {
            Vector3 targetPos = GenerateSpawnLocation();
            if (targetPos != Vector3.zero)
            {
                GameObject _target = Instantiate(enemy, targetPos, Quaternion.identity, enemyParent.transform);
                _target = _target.transform.GetChild(0).gameObject;
                _target.GetComponent<EnemyScript>().health = randomHealth;
                if (enemy.name == "Blinky")
                {
                    GeneratBlinkyPoses(_target);
                }
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


    private void GeneratBlinkyPoses(GameObject blunk)
    {
        for (float f = blunk.GetComponent<BlinkyScript>().health; f >= 0; f--)
        {
            for(int j = 0; j < 100000; j++)
            {
                Vector3 targetPos = GenerateSpawnLocation();
                if (targetPos != Vector3.zero)
                {
                    blunk.GetComponent<BlinkyScript>().blinkPositions.Add(targetPos);
                    break;
                }
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

    public void ClearEnemys()
    {
        EnemyScript[] allenemys = GetComponentsInChildren<EnemyScript>();
        foreach (EnemyScript _enemy in allenemys)
        {
            Destroy(_enemy.gameObject.transform.parent.gameObject);
        }
    }

}

