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
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    void GenerateUI()
    {
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
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

            Attachment _attachment = GameManager.instance.CreateAttachment(Rarity.NONE, AttachmentType.NONE);
            _shopUpgrade.GetComponent<ShopUpgradeScript>().attachment = _attachment;
        }
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

    // Roll item stuff




}
