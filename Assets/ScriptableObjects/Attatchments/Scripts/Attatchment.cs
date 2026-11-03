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
    DamageIncrease,
    DamageMult,
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
}