using UnityEngine;

public class TargetSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject targetSpawnArea;
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetParent;
    [SerializeField] int initialTargetCount;
    [SerializeField] int attemptsToSpawn = 1000;

    public Collider[] targetCollisions;
    public Collider[] spawnableCollisions;

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
            // The sqrt is to out to if a square was rotated 45 

            //float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2 * Mathf.Sqrt(2)) + XtargetSpawnAreaScale / 25, XtargetSpawnAreaScale / 2 * Mathf.Sqrt(2) - XtargetSpawnAreaScale / 25);
            //float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale/2 * Mathf.Sqrt(2)) + YtargetSpawnAreaScale/10, YtargetSpawnAreaScale/2 * Mathf.Sqrt(2) + YtargetSpawnAreaScale/25);
            //float targetZ = targetSpawnArea.transform.position.z + 0.3f; // just added offset
            //float targetX = 0;
            //float targetY = 5;
            float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2 * Mathf.Sqrt(2)) + XtargetSpawnAreaScale / 25, XtargetSpawnAreaScale / 2 * Mathf.Sqrt(2) - XtargetSpawnAreaScale / 25);
            float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale / 2 * Mathf.Sqrt(2)) + YtargetSpawnAreaScale / 10, YtargetSpawnAreaScale / 2 * Mathf.Sqrt(2) + YtargetSpawnAreaScale / 25);
            float targetZ = target.transform.GetChild(0).gameObject.GetComponent<SphereCollider>().radius + 0.1f;
            Vector3 targetPos = new Vector3(targetX, targetY, targetZ);
            //Debug.Log("targ z: " + targetZ);
            //Debug.Log("trans pos z: " + transform.position.z);
            //Debug.Log("test pos stuff: " + targetPos);
            
            //Collider[] Collisions;
            targetCollisions = Physics.OverlapSphere(targetPos, target.GetComponentInChildren<SphereCollider>().radius + targetSpace);
            spawnableCollisions = Physics.OverlapSphere(targetPos, 0.1f);
            //Collisions = Physics.OverlapSphere(targetPos, 0.5f, layerMask: 9);
            //GameObject _target = Instantiate(target, new Vector3(targetX,targetY,.55f), Quaternion.identity);
            //_target.GetComponent<Transform>().localScale = new Vector3(.1f,.1f,.1f);

            // number after layer is equal to the layer of TargetSpawnable
            //if ((targetCollisions.Length == 1 && (spawnableCollisions.Length == 0 || spawnableCollisions[0].gameObject.layer == 9)) || i == attemptsToSpawn - 1)

            //if ((targetCollisions[0].gameObject.layer == 9) || i == attemptsToSpawn - 1)
            //if ((targetCollisions.Length == 0 && spawnableCollisions.Length == 1))

            Debug.Log("length: " + targetCollisions.Length);
            Debug.Log("target coll: ");
            foreach (Collider coll in targetCollisions) { Debug.Log("targ coll: " + coll.name); }
            Debug.Log("spawnable collisions length: " + spawnableCollisions.Length);
            Debug.Log("spawnable collisions layer: " + spawnableCollisions[0].gameObject.layer);
            Debug.Log("colliders: ");
            foreach (Collider coll in spawnableCollisions) { Debug.Log("spawn coll: " + coll.name); }

            //if (spawnableCollisions.Length == 1) { Debug.Log("spawn call leng: 1"); Debug.Log("true"); }
            //else { Debug.Log("no spawn call leng not 1"); Debug.Log("false"); }
            //if (spawnableCollisions[0].gameObject.layer == 9) { Debug.Log("spawn coll 0 layer = 9"); Debug.Log("true"); }
            //else { Debug.Log("no spawn call 0 layer != 9"); Debug.Log("false"); }
            //if (targetCollisions.Length > 0) { Debug.Log("targ coll length > 0"); Debug.Log("true"); }
            //else { Debug.Log("no targ call length !> 0"); Debug.Log("false"); }
            //if (targetCollisions[0].gameObject.layer == 6) { Debug.Log("targ coll 0 layer = 6"); Debug.Log("true"); }
            //else { Debug.Log("no targ coll 0 layer != 6"); Debug.Log("false"); }
            //if (targetCollisions[1].gameObject.layer == 6) { Debug.Log("targ coll 1 layer = 6"); Debug.Log("true"); }
            //else { Debug.Log("no targ coll 1 layer != 6"); Debug.Log("false"); }

            if (spawnableCollisions.Length == 1 && spawnableCollisions[0].gameObject.layer == 9 && targetCollisions.Length > 0 && (targetCollisions[0].gameObject.layer != 6 || targetCollisions[1].gameObject.layer != 6))
            {
                //Debug.Log("layer: " + targetCollisions[0].gameObject.layer + " obj: " + targetCollisions[0].gameObject.name + " length: " + targetCollisions.Length);

                //Debug.Log("Test colliders: ");
                //foreach (Collider coll in Collisions) { Debug.Log("spawn coll: " + coll.name); }
                //foreach (Collider coll in targetCollisions) { Debug.Log("targ coll: " + coll.name); }
                //if (i == attemptsToSpawn-1)
                //{
                //    Debug.Log("force");
                //}
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
            //if (i == attemptsToSpawn -1)
            //{
            //    Debug.Log("force");
            //    GameObject _target = Instantiate(target, targetPos, Quaternion.identity, targetParent.transform);
            //    _target = _target.transform.GetChild(0).gameObject;
            //    if (areTargetsMoving)
            //    {
            //        Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
            //        float _targetSpeed = targetSpeed + Random.Range(-(targetSpeedVariability), targetSpeedVariability);
            //        _target.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);
            //        _target.GetComponent<TargetScript>().force = _targetSpeed;
            //    }
            //    return;
            //}
        }
        //Debug.LogError("Could not spawn target.");

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
