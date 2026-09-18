using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FlowerScript : TargetScript
{
    [SerializeField] GameObject petal;
    [SerializeField] int petalCount;
    [SerializeField] GameObject petalParent;
    [SerializeField] Vector3 petalOfset;
    List<GameObject> petals = new List<GameObject>();
    [SerializeField] float petalSpinSpeed;
    [SerializeField] bool petalsSpin;
    int degreasBetweenPetals;

    [SerializeField] int petalRotateVariationMax;
    int petalRotateVariationCurrent;
    private void Awake()
    {
        EventHandeler.onTargetDeath += CheckPetals;
        
    }
    private void Start()
    {
        spawnPetals();
        canGetHit = false;
        SetHealthBar();
        petalRotateVariationMax = Random.Range(1,petalRotateVariationMax +1);
        petalRotateVariationCurrent = petalRotateVariationMax;
        Debug.Log(petalRotateVariationMax);
    }
    private void Update()
    {
        petalParent.transform.position = gameObject.transform.position;
        healthBarCanvas.transform.position = gameObject.transform.position;
        if (!canGetHit)
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        petalParent.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, petalSpinSpeed);
    }
    void spawnPetals()
    {
        degreasBetweenPetals = 360 / petalCount;
        for (int i = petalCount; i > 0; i--)
        {
            GameObject newPetal = Instantiate(petal, petalParent.transform);
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
            canGetHit = true;
        }
    }
}
