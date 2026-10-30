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
        _attachment.rarity = (Rarity)Random.Range(0,5); // #TODO: rarity calculation
        RollStats(_attachment);

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

    // Roll item stuff
        public void RollStats(Attachment attachment)
    {
        // Debug.Log("Roll stats");
        // Debug.Log("mod count: " + modifiers.Count);
        foreach (Modifier modifier in attachment.modifiers)
        {
            Vector2 rollLimits = GetRollLimits(modifier.type, attachment);
            if (InventoryManager.instance.IsModTypePercentage(modifier.type))
            {
                modifier.value =  Mathf.Ceil(UnityEngine.Random.Range(rollLimits.x, rollLimits.y)); 
                return;   
            }
            modifier.value =  Mathf.Round(UnityEngine.Random.Range(rollLimits.x, rollLimits.y)*100)/100;
        }
    }


    Vector2 GetRollLimits(ModifierType modifierType, Attachment attachment)
    {
        switch(modifierType)
        {
            case ModifierType.ComboTime:
                // Debug.Log("roll comboTime");
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,1);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,1.75f);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(1, 2.5f);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(2,4);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(3, 6);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.DamageIncrease:
                // Debug.Log("roll damageInc");
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,5);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(2, 7);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(4, 9);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(7, 14);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(12,20);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.DamageMult:
                // Debug.Log("roll DamageMult");
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,30);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,60);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(0, 100);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(50, 130);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(100, 250);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.BulletSize:
                // Debug.Log("roll BulletSize")
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0, 100);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0, 200);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(100, 300);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(200, 350);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(300, 500);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.CritMult:
                // Debug.Log("roll CritMult")
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    // Returns value in percentage
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,30);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,60);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(0, 100);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(50, 130);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(100, 250);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.CritSize:
                // Debug.Log("roll CritSize")
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,30);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,60);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(0, 100);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(50, 130);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(100,250);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.TimeIncrease:
                // Debug.Log("roll TimeIncrease")
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0, 2);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(1, 3.5f);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(2, 4);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(3, 4.5f);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(5, 7);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.TimeMult:
                // Debug.Log("roll TimeMult")
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,3);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,5);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(0, 8);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(3, 10);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(10, 25);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.ComboIncrease:
                // Debug.Log("roll ComboIncrease")
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,2);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0, 4);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(2, 6);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(4, 9);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(5,12);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.ComboMult:
                // Debug.Log("roll ComboMult")
                // Returns value in percentage
                switch(attachment.rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0, 5);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,10);
                    case Rarity.RARE:
                        // Debug.Log("rare");
                        return new Vector2(5, 15);
                    case Rarity.EPIC:
                        // Debug.Log("epic");
                        return new Vector2(10, 20);
                    case Rarity.LEGENDARY:
                        // Debug.Log("Legendary");
                        return new Vector2(20, 30);
                    default:
                        return Vector2.zero;
                }
            default:
                return Vector2.zero;
        }
    }
}
