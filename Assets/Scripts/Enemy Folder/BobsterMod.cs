using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class BobsterMod : MonoBehaviour
{
    [SerializeField]GameObject prefab;
    float spawnInterval = 2f; // Adjust the spawn interval as needed
    void Start()
    {
        StartCoroutine(Mobster());
    }
    IEnumerator Mobster()
    {
        while (true)
        {

            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    GameObject Spawn()
    {
        GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation, transform.parent.parent.transform);
        newEnemy.transform.GetChild(0).AddComponent<SubEnemyScript>();
        Vector3 targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
        float targetSpeed = 3;
        newEnemy.GetComponentInChildren<Rigidbody>().AddForce(targetDirection.normalized * targetSpeed, ForceMode.Impulse);

        return newEnemy;
    }
}
