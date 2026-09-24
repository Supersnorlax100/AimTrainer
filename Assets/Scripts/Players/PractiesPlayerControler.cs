using UnityEngine;

public class PractiesPlayerControler : MonoBehaviour
{
    public static PractiesPlayerControler instance;

    // Shooting
    public GameObject target;
    Ray gunRay;
    RaycastHit targetHit;
    LayerMask targetHitMask;
    LayerMask targetCritMask;

    float shotTimer = 0.1f;
    float curShotTimer;

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

        curShotTimer = shotTimer;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused || GameManager.instance.forceLock)
        {
            return;
        }
        else if (!GameManager.instance.isPaused)
        {
            curShotTimer -= Time.deltaTime;
        }

        if (GameManager.instance.stageType == TargetType.CLICKING)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (curShotTimer <= 0)
                {
                    Shoot();
                    curShotTimer = shotTimer;
                }
            }
        }
    }

    void Shoot()
    {
        gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(gunRay.origin, gunRay.direction * 100f, Color.red, 1f);
        if (Physics.Raycast(gunRay, out targetHit, 9999999, targetCritMask))
        {
            target = targetHit.collider.transform.gameObject;
            if (target.GetComponent<Rigidbody>() == null)
            {
                target = target.transform.parent.gameObject;
            }

            target.GetComponent<TargetScript>().GetHit(damage, true, critMultiplier);
        }
        else if (Physics.Raycast(gunRay, out targetHit, 99999999, targetHitMask))
        {
            target = targetHit.collider.transform.gameObject;
            if (target.GetComponent<Rigidbody>() == null)
            {
                target = target.transform.parent.gameObject;
            }
            target.GetComponent<TargetScript>().GetHit(damage, false, critMultiplier);
        }
    }
}
