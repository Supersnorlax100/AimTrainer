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

public enum ModifierType
{
    DamageIncrease,
    DamageMult,
    CritChance,
    CritMult,
    CritSize,
    BulletSize,
    TimeIncrease,
    TimeMult,
    ComboTime,
    ComboIncrease,
    ComboMult
}

[Serializable]
public class Modifier
{
    public ModifierType type;
    public int value;
}

public abstract class Attatchment : ScriptableObject
{
    public Rarity rarity;
    public int id;
    public float price;

    public List<Modifier> modifiers = new();

    public int GetModifier(ModifierType type)
    {
        // for (int i = 0; i < Enum.GetNames(typeof(ModifierType)).Length; i++)
        // {
        //     Debug.Log("ugh");
        // }
        foreach (Modifier modifier in modifiers)
        {
            if (modifier.type == type)
                return modifier.value;
        }

        return 0;
    }
}