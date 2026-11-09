using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public enum EventModTypes
{
    // Should match attachment mods and event specific mods at end (score, etc...)
    DamageIncrease,
    DamageMult,
    CritMult,
    CritSize,
    BulletSize,
    TimeIncrease,
    TimeMult,
    ComboTime,
    ComboIncrease,
    ComboMult,
    Score
}
[Serializable]
public class EventModifier
{
    public EventModTypes type;
    public float value;
    public bool positive; // when true, the modifier is positive, when false, the modifier is negative
}


[Serializable]
// public class EventMods
// {
//     public List<EventModifier> eventModifier;
// }

[CreateAssetMenu(fileName = "EventForRoom", menuName = "Scriptable Objects/EventForRoom")]
public class EventForRoom : ScriptableObject
{
    public Image backgroundImage;
    public string eventText;
    public string[] options;
    public List<EventModifier> eventmodifiersO1;
    public List<EventModifier> eventmodifiersO2;
    public List<EventModifier> eventmodifiersO3;
    // public List<EventMods> eventMods;
}
