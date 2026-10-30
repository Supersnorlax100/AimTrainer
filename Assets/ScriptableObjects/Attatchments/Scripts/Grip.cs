using UnityEngine;

[CreateAssetMenu(fileName = "Grip", menuName = "Scriptable Objects/Attatchment/Grip")]
public class Grip : Attachment
{
    void Awake()
    {
        attachmentName = nameof(AttachmentType.Grip);

        if (modifiers.Count == 0)
        {
            CreateModifiers();
        }
    }

    public override void CreateModifiers()
    {
        Modifier _modifier = new Modifier{type = ModifierType.CritMult};
        Modifier _modifier2 = new Modifier{type = ModifierType.ComboMult};

        modifiers.Add(_modifier);
        modifiers.Add(_modifier2);
    }
}
