using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargetSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject targetSpawnArea;
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetParent;
    [SerializeField] int initialTargetCount;

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

    // Update is called once per frame
 


    public void Spawn()
    {
        for (int i = 0; i < 1000; i++)
        {
            float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2), XtargetSpawnAreaScale / 2);
            float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale / 2), YtargetSpawnAreaScale / 2);
            float targetZ = targetSpawnArea.transform.position.z + 0.75f; // just added offset
            Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

            if (Physics.OverlapSphere(targetPos, target.GetComponent<SphereCollider>().radius + targetSpace).Length <= 1)
            {
                GameObject _target = Instantiate(target, targetPos, Quaternion.identity, targetParent.transform);
                if (areTargetsMoving)
                {
                    Vector3 _targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
                    float _targetSpeed = targetSpeed + Random.Range(-(targetSpeedVariability), targetSpeedVariability);
                    _target.GetComponent<Rigidbody>().AddForce(_targetDirection.normalized * _targetSpeed, ForceMode.Impulse);
                }
                return;
            }
        }
        Debug.LogError("Could not spawn target.");
    }

}
