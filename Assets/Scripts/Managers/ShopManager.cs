using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    [SerializeField] GameObject shopUpgrade;
    [SerializeField] GameObject shopUpgradeContainer;
    GameObject[] shopUpgrades = new GameObject[3];

    [SerializeField] TMP_Text scoreText;

    void Awake()
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

    void Start()
    {
        GenerateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    void GenerateUI()
    {
        int upgrades = Random.Range(1, 4);
        for (int i = 0; i < upgrades; i++)
        {
            if (GameManager.instance.attatchmentPool.Length <= 0)
            {
                Debug.Log("no attatchments");
                return;   
            }
            
            GameObject _shopUpgrade = Instantiate(shopUpgrade, shopUpgradeContainer.transform);
            shopUpgrades.Append(_shopUpgrade);

            Attatchment _attatchment = GameManager.instance.attatchmentPool[Random.Range(0, GameManager.instance.attatchmentPool.Length)];
            _shopUpgrade.GetComponent<ShopUpgradeScript>().attatchment = _attatchment;
        }
    }

    public void Purchase(Attatchment attatchment)
    {
        if (GameManager.instance.score < (int)Mathf.Round(attatchment.price))
        {
            Debug.Log("brokie");
            return;
        }
        GameManager.instance.score -= (int)Mathf.Round(attatchment.price);
        EventHandeler.purchase?.Invoke();
        UpdateUI();
    }
}
