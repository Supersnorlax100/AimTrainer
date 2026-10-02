using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlowerModScript : MonoBehaviour
{
    [SerializeField] GameObject petal;
    [SerializeField] int petalCount;
    [SerializeField] GameObject petalParent;
    [SerializeField] Vector3 petalOfset;
    [SerializeField] GameObject[] petalPrefabs;
    List<GameObject> petals = new List<GameObject>();
    [SerializeField] float petalSpinSpeed;
    [SerializeField] bool petalsSpin;
    int degreasBetweenPetals;

    [SerializeField] int petalRotateVariationMax;
    int petalRotateVariationCurrent;

    public GameObject spawnAreaForPetals;

    private void Awake()
    {
        EventHandeler.onSubEnemyDeath += CheckPetals;

    }

    private void Start()
    {
        SpawnPetals();
        GetComponent<EnemyScript>().canGetHit = false;
        petalRotateVariationMax = Random.Range(1, petalRotateVariationMax + 1);
        petalRotateVariationCurrent = petalRotateVariationMax;
    }


    public void Update()
    {
        petalParent.transform.position = gameObject.transform.position;
        petalParent.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, petalSpinSpeed);
    }
    public void SpawnPetals()
    {

        degreasBetweenPetals = 360 / petalCount;
        for (int i = petalCount; i > 0; i--)
        {
            GameObject petalPrefab = petalPrefabs[Random.Range(0, petalPrefabs.Length)];
            GameObject newPetal = Instantiate(petalPrefab, petalParent.transform);
            if (newPetal.GetComponentInChildren<BlinkyModScript>())
            {
                newPetal.GetComponentInChildren<BlinkyModScript>().spawnArea = spawnAreaForPetals;
            }
            GameManager.instance.targetCount++;
            petalParent.transform.rotation = Quaternion.Euler(0, 0, degreasBetweenPetals * i);
            newPetal.gameObject.transform.position = gameObject.transform.position + petalOfset;
            petals.Add(newPetal);

        }
    }



    public async void CheckPetals()
    {
        await System.Threading.Tasks.Task.Delay(10);
        for (int i = 0; i < petals.Count; i++)
        {
            if (petals[i] == null)
            {
                petals.RemoveAt(i);
                petalRotateVariationCurrent--;
                if (petalRotateVariationCurrent <= 0)
                {

                    petalSpinSpeed *= -1;
                    petalRotateVariationCurrent = petalRotateVariationMax;
                }


            }
        }
        if (petals.Count == 0)
        {
            GetComponent<EnemyScript>().canGetHit = true;
        }
    }
}
