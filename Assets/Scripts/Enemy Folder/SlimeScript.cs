using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

public class SlimeScript : MonoBehaviour
{
    int maxHealth;
    public float numberOFSplit;
    public Transform parent;
    public GameObject prefab;
    private void Start()
    {
        if (parent == null || parent.name != "SlimeParent")
        {
            parent = new GameObject("SlimeParent").transform;
            transform.parent.parent = parent;
        }
    }
    private void OnDestroy()
    {
        Debug.Log("Ns: "+numberOFSplit);
        Debug.Log("PL: "+parent.GetComponentsInChildren<Transform>().Length);
        if (numberOFSplit <= 0 && parent.GetComponentsInChildren<Transform>().Length == 1)
        {
            Debug.Log("Slime has no more splits and no sibling, invoking onEnemyDeath");
            EventHandeler.onEnemyDeath?.Invoke();
            return;
        }
        if (numberOFSplit <= 0)
        {
            return;
        }
        GameObject slimeBall_1 = Spawn();
        GameObject slimeBall_2 = Spawn();

        if (maxHealth / 2 >= 1)
        {
            slimeBall_1.GetComponentInChildren<EnemyScript>().health = maxHealth / 2;
            slimeBall_2.GetComponentInChildren<EnemyScript>().health = maxHealth / 2;
        }
        else
        {
            slimeBall_1.GetComponentInChildren<EnemyScript>().health = 1;
            slimeBall_2.GetComponentInChildren<EnemyScript>().health = 1;
        }
        slimeBall_1.GetComponentInChildren<SlimeScript>().numberOFSplit = numberOFSplit - 1;
        slimeBall_2.GetComponentInChildren<SlimeScript>().numberOFSplit = numberOFSplit - 1;
    }
    GameObject Spawn()
    {
        GameObject slimeBall_1 = Instantiate(prefab, transform.position, transform.rotation, parent);
        slimeBall_1.transform.GetChild(0).AddComponent<SlimeScript>();
        slimeBall_1.transform.GetChild(0).AddComponent<SubEnemyScript>();
        Vector3 targetDirection = new Vector3(Random.Range(0, 10), Random.Range(1, 10), 0);
        float targetSpeed = 3;
        slimeBall_1.GetComponentInChildren<Rigidbody>().AddForce(targetDirection.normalized * targetSpeed, ForceMode.Impulse);
        slimeBall_1.GetComponentInChildren<SlimeScript>().prefab = prefab;
        slimeBall_1.GetComponentInChildren<SlimeScript>().parent = parent;

        return slimeBall_1;
    }
}
