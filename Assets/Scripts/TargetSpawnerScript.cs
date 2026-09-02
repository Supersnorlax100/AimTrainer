using UnityEngine;

public class TargetSpawnerScript : MonoBehaviour
{
    public GameObject targetSpawnArea;
    public GameObject target;
    public GameObject targetParent;

    public float targetSpace;

    private float XtargetSpawnAreaScale;
    private float YtargetSpawnAreaScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        XtargetSpawnAreaScale = targetSpawnArea.transform.localScale.x;
        YtargetSpawnAreaScale = targetSpawnArea.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Spawn();   
            Debug.Log("space");
        }
    }


    void Spawn()
    {
        for (int i = 0; i < 20; i++)
        {
            float targetX = targetSpawnArea.transform.position.x + Random.Range(-(XtargetSpawnAreaScale / 2), XtargetSpawnAreaScale / 2);
            float targetY = targetSpawnArea.transform.position.y + Random.Range(-(YtargetSpawnAreaScale / 2), YtargetSpawnAreaScale / 2);
            float targetZ = targetSpawnArea.transform.position.z + 0.75f; // just added offset
            Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

            if (Physics.OverlapSphere(targetPos, target.GetComponent<SphereCollider>().radius + targetSpace).Length == 0)
            {
                Debug.Log("no overlap");
                Instantiate(target, targetPos, Quaternion.identity, targetParent.transform);
                break;
            }
        }
    }

}
