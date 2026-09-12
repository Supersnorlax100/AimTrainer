using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler instance;

    // Shooting
    public GameObject target;
    Ray gunRay;
    RaycastHit targetHit;
    LayerMask targetHitMask;
    LayerMask targetCritMask;

    public float mouseSens;

    public float damage = 1;
    public float critMultiplier = 1.5f;

    public int score = 0;
    public float roomScore = 0;

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

        targetHitMask = LayerMask.GetMask("TargetHit");
        targetCritMask = LayerMask.GetMask("TargetCrit");
    }

    private void Update()
    {
        if (GameManager.instance.isPaused || GameManager.instance.forceLock)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(gunRay.origin, gunRay.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(gunRay, out targetHit, 9999999, targetCritMask))
            {
                target = targetHit.transform.gameObject;

                target.GetComponent<TargetScript>().GetHit(damage, true, critMultiplier);
            }
            else if (Physics.Raycast(gunRay, out targetHit, 99999999, targetHitMask))
            {
                target = targetHit.transform.gameObject;

                target.GetComponent<TargetScript>().GetHit(damage, false, critMultiplier);
            }

        }
    }
}
