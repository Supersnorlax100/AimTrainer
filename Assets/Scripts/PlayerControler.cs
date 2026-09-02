using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler instance;
    Ray gunRay;
    RaycastHit whatHit;

    [SerializeField] int damage;

    int score = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(gunRay.origin, gunRay.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(gunRay, out whatHit) && whatHit.transform.gameObject.GetComponent<TargetScript>() != null)
            {
                GameObject target = whatHit.transform.gameObject;

                target.GetComponent<TargetScript>().GetHit(damage);

            }

        }
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }

}
