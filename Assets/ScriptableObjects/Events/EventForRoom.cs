using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public enum EventModTypes
{
    Score,
    Damage,
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
public class EventModifier
{
    public EventModTypes type;
    public float value;
    public bool positive; // when true, the modifier is positive, when false, the modifier is negative
}
[CreateAssetMenu(fileName = "EventForRoom", menuName = "Scriptable Objects/EventForRoom")]
public class EventForRoom : ScriptableObject
{
    public Image backgroundImage;
    public string eventText;
    public int optionCount;
    public string[] options;
    public List<EventModifier> eventmodifiersO1;
    public List<EventModifier> eventmodifiersO2;
    public List<EventModifier> eventmodifiersO3;
}
