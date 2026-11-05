using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemyPrefab;

    Component[] modScripts;

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
        modScripts = GameManager.instance.modScriptsUnlocked.ToArray();
        for (int i = 0; i < initialEnemyCount; i++)  Spawn();
    }

    private void OnDestroy()
    {
        EventHandeler.onEnemyDeath -= Spawn;
    }

    private float GenerateRandomHealth(EnemyScript enemy)
    {
        enemy.health += 1 * enemy.modScripts.Count;
        float maxHealth = (float)Math.Pow(enemy.health + (float)MapManager.instance.floorsCleared/3, 1 + (float)MapManager.instance.currnetLvl/3);
        // float maxHealth = (enemy.health +  MapManager.instance.currnetLvl * 3) * (1 + MapManager.instance.currentRoom.y / 10);
        // float maxHealth = Mathf.Pow(1.3f, MapManager.instance.currentRoom.y) * 5;
        Debug.Log((enemy.health + (float)MapManager.instance.floorsCleared/3) + "^" + (1+ MapManager.instance.currnetLvl));
        Debug.Log("max health: " + maxHealth);
        return UnityEngine.Random.Range(maxHealth * 0.8f, maxHealth); 
    }
    public Vector3 GenerateSpawnLocation()
    {
        float targetX = spawnArea.transform.position.x + UnityEngine.Random.Range(-(XspawnAreaScale / 2) + XspawnAreaScale / 25, XspawnAreaScale / 2 - XspawnAreaScale / 25);
        float targetY = spawnArea.transform.position.y + UnityEngine.Random.Range(-(YspawnAreaScale / 2), YspawnAreaScale / 2);
        float targetZ = spawnArea.transform.position.z + enemyPrefab.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().radius - .2f;
        Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

        // Test a collider in given area
        targetCollisions = Physics.OverlapSphere(targetPos, enemyPrefab.GetComponentInChildren<SphereCollider>().radius + enemySpace);
        spawnableCollisions = Physics.OverlapSphere(targetPos, 0.1f);

        #region Debugs
        //Debug.Log("atemted spawn pos: " + targetPos);
        //Debug.Log("length: " + targetCollisions.Length);
        //Debug.Log("target coll: ");
        //foreach (Collider coll in targetCollisions) { Debug.Log("targ coll: " + coll.name); }
        //Debug.Log("spawnable collisions length: " + spawnableCollisions.Length);
        //Debug.Log("spawnable collisions layer: " + spawnableCollisions[0].gameObject.layer);
        //Debug.Log("colliders: ");
        //foreach (Collider coll in spawnableCollisions) { Debug.Log("spawn coll: " + coll.name); }
        #endregion

        if (spawnableCollisions.Length == 1 && spawnableCollisions[0].gameObject.layer == 9 && targetCollisions.Length > 0 && (targetCollisions[0].gameObject.layer != 6 || targetCollisions[1].gameObject.layer != 6))
        {
            return targetPos;
        }
        return Vector3.zero;
    }

    public void Spawn()
    {
        if (numberOfSpawnedEnemys >= maxEnemyCount && GameManager.instance.targetCount <= 0)
        {
            // Debug.Log(GameManager.instance.targetCount);
            EventHandeler.exitRoom?.Invoke();
        }
        else if (numberOfSpawnedEnemys >= maxEnemyCount)
        {
            return; // have spawnd all enemys it has but player has not killed all of them yet.
        }
        for (int i = 0; i < attemptsToSpawn; i++)
        {
            Vector3 targetPos = GenerateSpawnLocation();
            if (targetPos != Vector3.zero)
            {
                MakeEnemy(targetPos);
                return;
            }
            if (i == attemptsToSpawn - 1)
            {
                Debug.Log("force");
                GenerateSpawnLocation();
                MakeEnemy(targetPos);
                return;
            }
        }
    }

    void MakeEnemy(Vector3 pos)
    {
        numberOfSpawnedEnemys++;
        GameManager.instance.targetCount++;
        GameObject newEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity, enemyParent.transform);
        newEnemy = newEnemy.transform.GetChild(0).gameObject;
        newEnemy.GetComponent<EnemyScript>().health = GenerateRandomHealth(newEnemy.GetComponent<EnemyScript>());
        foreach(MonoBehaviour script in modScripts)
        {
            newEnemy.AddComponent(script.GetType());
            newEnemy.GetComponent<EnemyScript>().scoreValue += 10;
        }

        SetSpawnAreas(newEnemy.gameObject);
        GiveEnemyPreFab(newEnemy.gameObject);
        float _critSize = GameManager.instance.critSize;
        newEnemy.gameObject.transform.GetChild(0).gameObject.transform.localScale = new Vector3(_critSize, _critSize, _critSize);
        if (areEnemysMoving)
        {
            Vector3 _targetDirection = new Vector3(UnityEngine.Random.Range(0, 10), UnityEngine.Random.Range(1, 10), 0);
            float _targetSpeed = enemySpeed + UnityEngine.Random.Range(-enemySpeedVariability, enemySpeedVariability);
            newEnemy.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);
        }
    }

    void SetSpawnAreas(GameObject enemy)
    {
        if (enemy.GetComponent<BlinkyModScript>())
        {
            enemy.GetComponent<BlinkyModScript>().spawnArea = spawnArea;
        }
        if (enemy.GetComponent<FlowerModScript>())
        {
            enemy.GetComponent<FlowerModScript>().spawnArea = spawnArea;
        }
    }

    void GiveEnemyPreFab(GameObject enemy)
    {
        if (enemy.GetComponent<SlimeScript>())
        {
            enemy.GetComponent<SlimeScript>().prefab = enemyPrefab;
        }
        if (enemy.GetComponent<SnorlaxMod>())
        {
            enemy.GetComponent<SnorlaxMod>().prefab = enemyPrefab;
        }
        if (enemy.GetComponent<BobsterMod>())
        {
            enemy.GetComponent<BobsterMod>().prefab = enemyPrefab;
        }
        if (enemy.GetComponent<FlowerModScript>())
        {
            enemy.GetComponent<FlowerModScript>().prefab = enemyPrefab;
        }

    }
    public void RespawnAll()
    {
        for (int i = 0; i < initialEnemyCount; i++)  Spawn();
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