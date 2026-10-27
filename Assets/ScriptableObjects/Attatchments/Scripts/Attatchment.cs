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

    public void RollStats()
    {
        // Debug.Log("Roll stats");
        // Debug.Log("mod count: " + modifiers.Count);
        foreach (Modifier modifier in modifiers)
        {
            Vector2 rollLimits = GetRollLimits(modifier.type);
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
                        return new Vector2(0,2);
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
                        return new Vector2(0,10);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.DamageMult:
                // Debug.Log("roll DamageMult");
                switch(rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,1.1f);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,3);
                    default:
                        return Vector2.zero;
                }
            case ModifierType.BulletSize:
                // Debug.Log("roll ButteltSize");
                switch(rarity)
                {
                    case Rarity.COMMON:
                        // Debug.Log("common");
                        return new Vector2(0,1);
                    case Rarity.UNCOMMON:
                        // Debug.Log("uncommon");
                        return new Vector2(0,2);
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