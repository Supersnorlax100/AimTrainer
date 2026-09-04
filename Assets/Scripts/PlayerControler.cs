using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public static PlayerControler instance;
    Ray gunRay;
    RaycastHit targetHit;
    RaycastHit targetCrit;
    GameObject target;

    LayerMask targetHitMask;
    LayerMask targetCritMask;

    public float damage = 1;
    public float critMultiplier = 1.5f;

    public int score = 0;

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
        EventHandeler.onTargetDeath += AddScore;

        targetHitMask = LayerMask.GetMask("TargetHit");
        targetCritMask = LayerMask.GetMask("TargetCrit");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(gunRay.origin, gunRay.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(gunRay, out targetHit, 9999999, targetCritMask))
            {
                target = targetHit.transform.gameObject;

                //Debug.Log(target.name);
                target.GetComponent<TargetScript>().GetHit(damage, true, critMultiplier);
            }
            else if (Physics.Raycast(gunRay, out targetHit, 99999999, targetHitMask))
            {
                target = targetHit.transform.gameObject;

                //Debug.Log(target.name);
                target.GetComponent<TargetScript>().GetHit(damage, false, critMultiplier);
            }

        }
    }

    public void AddScore()
    {
        score += target.GetComponent<TargetScript>().scoreValue;
        scoreText.text = "Score: " + score.ToString();
    }

}
