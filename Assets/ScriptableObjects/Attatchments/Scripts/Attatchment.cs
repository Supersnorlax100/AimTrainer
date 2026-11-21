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
    ComboTime,
    ComboIncrease,
    ComboMult,
    CritMult,
    CritSize,
    TimeIncrease,
    TimeMult,
    BulletSize
}

[Serializable]
public class Modifier
{
    public ModifierType type;
    public float value;
}

public abstract class Attachment : ScriptableObject
{
    public string attachmentName;
    public Rarity rarity;
    public int id;
    public float price;
    public AttachmentType type;

    // Don't touch modifiers in editor
    [HideInInspector] public List<Modifier> modifiers;

    void Awake()
    {
        CreateModifiers();
    }

    public abstract void CreateModifiers();
}