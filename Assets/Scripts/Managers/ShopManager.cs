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
        #if UNITY_EDITOR
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
        #endif
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
                Debug.LogError("no attachments");
                return;   
            }
            
            GameObject _shopUpgrade = Instantiate(shopUpgrade, shopUpgradeContainer.transform);
            shopUpgrades[i] = _shopUpgrade;

            Attachment _attachment = GameManager.instance.CreateAttachment(Rarity.NONE, AttachmentType.NONE);
            _shopUpgrade.GetComponent<ShopUpgradeScript>().attachment = _attachment;
        }
    }

    Attachment CreateAttachment()
    {
        Attachment _attachment = Instantiate(GameManager.instance.attachmentPool[Random.Range(0, GameManager.instance.attachmentPool.Length)]);
        int _rarity = Random.Range(0,101);
        /*
        5% Legendary
        10% Epic
        15% Rare
        30% Uncommon
        40% Common
        */
        // Debug.Log("rarity num: " + _rarity);
        // Debug.Log("rarity after sub: " + (_rarity - MapManager.instance.floorsCleared));
        if      (_rarity <= 45 - MapManager.instance.floorsCleared)  {_attachment.rarity = Rarity.COMMON;}
        else if (_rarity <= 75 - MapManager.instance.floorsCleared)  {_attachment.rarity = Rarity.UNCOMMON;}
        else if (_rarity <= 90 - MapManager.instance.floorsCleared)  {_attachment.rarity = Rarity.RARE;}
        else if (_rarity <= 100 - MapManager.instance.floorsCleared)  {_attachment.rarity = Rarity.EPIC;}
        else if (_rarity <= 105 - MapManager.instance.floorsCleared) {_attachment.rarity = Rarity.LEGENDARY;}
        else {Debug.LogError("rarity calculation failure");}
        
        RollStats(_attachment);
        return _attachment;
    }

    public void Purchase(Attachment attachment)
    {
        if (! CanPurchase(attachment, true))
        {
            return;
        }
        AudioManager.instance.PurchaseSound();
        GameManager.instance.score -= Mathf.CeilToInt(attachment.price);
        InventoryManager.instance.EquipAttachment(attachment);
        EventHandeler.purchase?.Invoke();
        UpdateUI();
        Debug.Log("inventory after purchase: ");
        InventoryManager.instance.DebugInventory();
    }

    public bool CanPurchase(Attachment attachment, bool showPrompt)
    {
        if (InventoryManager.instance.CheckInventorySlots(attachment.type) == -1 && InventoryManager.instance.GetEquipedAttachment(attachment.type) != null)
        {
            if (showPrompt) UiManager.instance.InfoPrompt(true, "Your inventory is full");
            
            return false;
        }
        if (GameManager.instance.score < Mathf.CeilToInt(attachment.price))
        {
            if (showPrompt) UiManager.instance.InfoPrompt(true, "You do not have enough score");
            
            return false;
        }
        return true;
    }

    // Roll item stuff




}
