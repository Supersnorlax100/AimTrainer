using UnityEngine;
using TMPro;

public class PlayerControler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public static PlayerControler instance;
    Ray gunRay;
    RaycastHit whatHit;
    GameObject target;

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
        EventHandeler.onTargetDeath += AddScore;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            gunRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(gunRay.origin, gunRay.direction * 100f, Color.red, 1f);
            if (Physics.Raycast(gunRay, out whatHit) && whatHit.transform.gameObject.GetComponent<TargetScript>() != null)
            {
                target = whatHit.transform.gameObject;

                target.GetComponent<TargetScript>().GetHit(damage);

            }

        }
    }

    public void AddScore()
    {
        score += target.GetComponent<TargetScript>().scoreValue;
        scoreText.text = "Score: " + score.ToString();
    }

}
