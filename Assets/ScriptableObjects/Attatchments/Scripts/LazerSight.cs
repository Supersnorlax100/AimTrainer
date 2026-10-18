using UnityEngine;

[CreateAssetMenu(fileName = "Lazer Sight", menuName = "Scriptable Objects/Attatchment/Lazer Sight")]
public class LazerSight : Attachment
{
    void Awake()
    {
        Modifier lazerMod = new Modifier{type = ModifierType.ComboTime};
        modifiers.Add(lazerMod);
    }
}
