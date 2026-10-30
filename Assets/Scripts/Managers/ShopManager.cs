using System.Collections.Generic;
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

    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            // Reroll shop
            for (int i = 0; i < shopUpgrades.Length; i++)
            {
                Destroy(shopUpgrades[i]);
                shopUpgrades[i] = null;
            }
            GenerateUI();
        }
    }

    void UpdateUI()
    {
        Debug.Log("update ui, game score: " + GameManager.instance.score.ToString());
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    void GenerateUI()
    {
        int numOfUpgrades = Random.Range(1, 4);
        for (int i = 0; i < numOfUpgrades; i++)
        {
            if (GameManager.instance.attachmentPool.Length <= 0)
            {
                Debug.Log("no attachments");
                return;   
            }
            
            GameObject _shopUpgrade = Instantiate(shopUpgrade, shopUpgradeContainer.transform);
            shopUpgrades[i] = _shopUpgrade;

            Attachment _attachment = CreateAttachment();
            _shopUpgrade.GetComponent<ShopUpgradeScript>().attachment = _attachment;
        }
    }

    Attachment CreateAttachment()
    {
        Attachment _attachment = Instantiate(GameManager.instance.attachmentPool[Random.Range(0, GameManager.instance.attachmentPool.Length)]);
        _attachment.rarity = (Rarity)Random.Range(0,2); // #TODO: rarity calculation
        _attachment.RollStats();

        return _attachment;
    }

    public void Purchase(Attachment attachment)
    {
        if (InventoryManager.instance.CheckInventorySlots(attachment.type) == -1 && InventoryManager.instance.GetEquipedAttachment(attachment.type) != null)
        {
            Debug.Log("inventory full");
            return;
        }
        if (GameManager.instance.score < Mathf.CeilToInt(attachment.price))
        {
            Debug.Log("brokie");
            return;
        }
        GameManager.instance.score -= Mathf.CeilToInt(attachment.price);
        InventoryManager.instance.EquipAttachment(attachment);
        EventHandeler.purchase?.Invoke();
        UpdateUI();
        Debug.Log("inventory after purchase: ");
        InventoryManager.instance.DebugInventory();
    }
}
