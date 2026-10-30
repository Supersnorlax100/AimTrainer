using System;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    LEGENDARY
}

public enum AttachmentType
{
    Optic,
    Grip,
    Mag,
    Barrel,
    Lazer,
    Trigger
}

//#TODO: Make bullet size work
public enum ModifierType
{
    DamageIncrease, //damageIncrease 
    DamageMult, //damageMult 
    CritMult,  //critMult 
    CritSize,
    BulletSize,
    TimeIncrease, //Just add to bonusTime
    TimeMult, // timeMult
    ComboTime, //maxComboTimer 
    ComboIncrease, // comboIncrease 
    ComboMult // comboMultAddition 
}

[Serializable]
public class Modifier
{
    [HideInInspector] public ModifierType type;
    [HideInInspector] public float value;
}

public abstract class Attachment : ScriptableObject
{
    public string attachmentName;
    public Rarity rarity;
    public int id;
    public float price;
    public AttachmentType type;

    [Header("NO TOUCH NO TOUCH NO TOUCH NO TOUCH NO TOUCH NO TOUCH ")] // Don't touch modifiers in editor

    public List<Modifier> modifiers;

    void Awake()
    {
        CreateModifiers();
    }

    public abstract void CreateModifiers();

    //#TODO: Move roll stats outside of the attachment itself
    public void RollStats()
    {
        // Debug.Log("Roll stats");
        // Debug.Log("mod count: " + modifiers.Count);
        foreach (Modifier modifier in modifiers)
        {
            Vector2 rollLimits = GetRollLimits(modifier.type);
            if (InventoryManager.instance.IsModTypePercentage(modifier.type))
            {
                modifier.value =  Mathf.Ceil(UnityEngine.Random.Range(rollLimits.x, rollLimits.y)); 
                return;   
            }
            modifier.value =  Mathf.Round(UnityEngine.Random.Range(rollLimits.x, rollLimits.y)*100)/100;
        }
    }


    Vector2 GetRollLimits(ModifierType modifierType)
    {
        switch(modifierType)
        {
            case ModifierType.ComboTime:
                // Debug.Log("roll comboTime");
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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
                switch(rarity)
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

    // public int GetModifier(ModifierType type)
    // {

    //     foreach (Modifier modifier in modifiers)
    //     {
    //         if (modifier.type == type)
    //             return modifier.value;
    //     }

    //     return 0;
    // }
}