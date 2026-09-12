using UnityEngine;

public class TargetSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject targetSpawnArea;
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetParent;
    [SerializeField] int initialTargetCount;
    [SerializeField] int attemptsToSpawn = 1000;

    public float targetSpace;

    private float XtargetSpawnAreaScale;
    private float YtargetSpawnAreaScale;

    public bool areTargetsMoving;
    public float targetSpeed = 1;
    public float targetSpeedVariability = 0;

    private void Awake()
    {
        EventHandeler.onTargetDeath += Spawn;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XtargetSpawnAreaScale = targetSpawnArea.transform.localScale.x;
        YtargetSpawnAreaScale = targetSpawnArea.transform.localScale.y;

        for (int i = 0; i < initialTargetCount; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < attemptsToSpawn; i++)
        {
            // The + YtargetSpawnAreaScale/10 is because the target spawns a little too low than what it should so I added an offset
            // float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2), XtargetSpawnAreaScale / 2);
            // float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale / 2), YtargetSpawnAreaScale / 2);
            float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2) + XtargetSpawnAreaScale/25, XtargetSpawnAreaScale / 2 - XtargetSpawnAreaScale/25);
            float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale / 2) + YtargetSpawnAreaScale/10, YtargetSpawnAreaScale / 2 + YtargetSpawnAreaScale/25);
            float targetZ = targetSpawnArea.transform.position.z + 0.3f; // just added offset
            Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

            if (Physics.OverlapSphere(targetPos, target.GetComponentInChildren<SphereCollider>().radius + targetSpace).Length == 1 || i == attemptsToSpawn-1)
            {
                GameObject _target = Instantiate(target, targetPos, Quaternion.identity, targetParent.transform);
                _target = _target.transform.GetChild(0).gameObject;
                if (areTargetsMoving)
                {
                    Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
                    float _targetSpeed = targetSpeed + Random.Range(-(targetSpeedVariability), targetSpeedVariability);
                    _target.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);
                    _target.GetComponent<TargetScript>().force = _targetSpeed;
                }
                return;
            }
        }
        Debug.LogError("Could not spawn target.");

    }

    public void RespawnAll()
    {
        for (int i = 0; i < initialTargetCount; i++)
        {
            Spawn();
        }
    }

    public void ClearTargets()
    {
        TargetScript[] allTargets = GetComponentsInChildren<TargetScript>();
        foreach (TargetScript _target in allTargets)
        {
            Destroy(_target.gameObject.transform.parent.gameObject);
        }
    }

}
