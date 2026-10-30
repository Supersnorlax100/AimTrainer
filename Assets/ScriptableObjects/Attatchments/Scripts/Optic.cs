using UnityEngine;

[CreateAssetMenu(fileName = "Optic", menuName = "Scriptable Objects/Attatchment/Optic")]
public class Optic : Attachment
{
    void Awake()
    {
        attachmentName = nameof(AttachmentType.Optic);

        if (modifiers.Count == 0)
        {
            CreateModifiers();
        }
    }

    public override void CreateModifiers()
    {
        Modifier _modifier = new Modifier{type = ModifierType.CritMult};
        Modifier _modifier2 = new Modifier{type = ModifierType.CritSize};

        modifiers.Add(_modifier);
        modifiers.Add(_modifier2);
    }
}
