using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public static PlayerControler instance;

    // Shooting
    public GameObject target;
    Ray gunRay;
    RaycastHit targetHit;
    LayerMask targetHitMask;
    LayerMask targetCritMask;

    float shotTimer = 0.1f;
    float curShotTimer;

    public float mouseSens;

    public float critMultiplier = 1.5f;

    int shotsHit;
    int shotsMissed;

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

        EventHandeler.exitRoom += ExitRoom;
        EventHandeler.enterRoom += EnterRoom;

        targetHitMask = LayerMask.GetMask("TargetHit");
        targetCritMask = LayerMask.GetMask("TargetCrit");

        curShotTimer = shotTimer;
    }

    void Start()
    {
        mouseSens = SettingsManager.instance.mouseSens;
    }

    private void Update()
    {
        if (GameManager.instance.isPaused || GameManager.instance.forceLock)
        {
            return;
        }
        else if (! GameManager.instance.isPaused)
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
            if (target.GetComponent<EnemyScript>() == null)
            {
                target = target?.transform?.parent?.gameObject;
            }
            shotsHit++;
            target.GetComponent<EnemyScript>()?.GetHit(DamageCalc(true));
            AudioManager.instance.PlayHitSound();
            // Debug.Log("Crit Hit");
        }
        else if (Physics.Raycast(gunRay, out targetHit, 99999999, targetHitMask))
        {
            target = targetHit.collider.transform.gameObject;
            if(target.GetComponent<EnemyScript>() == null)
            {
                target = target?.transform?.parent?.gameObject;
            }
            target.GetComponent<EnemyScript>()?.GetHit(DamageCalc(false));
            shotsHit++;
            // Debug.Log("Hit");
            AudioManager.instance.PlayHitSound();
        }
        else {shotsMissed++;}
        AccuracyCalc();
    }

    float DamageCalc(bool isCrit)
    {
        float damage = GameManager.instance.damage;
        if (isCrit)
        {
            damage *= GameManager.instance.critMult;
        }
        // Debug.Log("Damage: " + damage);
        return damage;
    }

    public void EnterRoom()
    {
        shotsHit = 0;
        shotsMissed = 0;
        GameManager.instance.accuracy = 0;
        UiManager.instance.UpdateScoreAndAccuracyText();
    }

    public void ExitRoom()
    {
        AccuracyCalc();
    }

    public void AccuracyCalc()
    {
        // Debug.Log("hit: " + shotsHit + " miss: " + shotsMissed + " comb: " + (shotsMissed+shotsHit) + " accCalc: " + ((float)shotsHit/(shotsMissed+shotsHit)));
        GameManager.instance.accuracy = (float)shotsHit/(shotsMissed + shotsHit);
        UiManager.instance.UpdateScoreAndAccuracyText();
    }
}
