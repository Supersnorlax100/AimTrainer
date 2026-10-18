using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Barrel", menuName = "Scriptable Objects/Attatchment/Barrel")]
public class Barrel : Attachment
{
    // For attatchment available mods
    void Awake()
    {
        Modifier _modifier = new Modifier{type = ModifierType.DamageIncrease};
        Modifier _modifier2 = new Modifier{type = ModifierType.DamageMult};
        Modifier _modifier3 = new Modifier{type = ModifierType.BulletSize};

        modifiers.Add(_modifier);
        modifiers.Add(_modifier2);
        modifiers.Add(_modifier3);
    }
}
