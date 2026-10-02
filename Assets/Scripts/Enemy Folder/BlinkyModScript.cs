using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkyModScript : MonoBehaviour
{
    public GameObject spawnArea;
    public List<Vector3> blinkPositions;
    private Vector3 nextLocation;
    private float XspawnAreaScale;
    private float YspawnAreaScale;

    public void Start()
    {
        XspawnAreaScale = spawnArea.transform.localScale.x;
        YspawnAreaScale = spawnArea.transform.localScale.y;
        StartCoroutine(FindSpawn());
    }

    public void GetHit()
    {
        transform.position = nextLocation;
        transform.parent.parent = null;
        transform.parent.rotation = Quaternion.Euler(0, 0, 0);
        if (GetComponent<FlowerModScript>())
        {
            GetComponent<FlowerModScript>().Invoke("SpawnPetals",0.1f);
            GetComponent<EnemyScript>().canGetHit = false;
        }
        StartCoroutine(FindSpawn());
    }


    IEnumerator FindSpawn()
    {
        while (nextLocation == Vector3.zero)
        {
            nextLocation = GenerateSpawnLocation();
            yield return null;
        }
        StopCoroutine(FindSpawn());
    }


    public Vector3 GenerateSpawnLocation()
    {
        Collider[] targetCollisions;
        Collider[] spawnableCollisions;
        // The + YtargetSpawnAreaScale/10 is because the target spawns a little too low than what it should so I added an offset
        // The sqrt is to out to if a square was rotated 45             
        float targetX = spawnArea.transform.position.x + Random.Range(-(XspawnAreaScale / 2) + XspawnAreaScale / 25, XspawnAreaScale / 2 - XspawnAreaScale / 25);
        float targetY = spawnArea.transform.position.y + Random.Range(-(YspawnAreaScale / 2), YspawnAreaScale / 2);
        float targetZ = spawnArea.transform.position.z + GetComponent<SphereCollider>().radius - .2f;
        Vector3 targetPos = new Vector3(targetX, targetY, targetZ);

        // Test a collider in given area
        targetCollisions = Physics.OverlapSphere(targetPos, GetComponent<SphereCollider>().radius);
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
}
