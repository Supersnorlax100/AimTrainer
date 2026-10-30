using UnityEngine;

[CreateAssetMenu(fileName = "Mag", menuName = "Scriptable Objects/Attatchment/Mag")]
public class Mag : Attachment
{
    void Awake()
    {
        attachmentName = nameof(AttachmentType.Mag);

        if (modifiers.Count == 0)
        {
            CreateModifiers();
        }
    }

    public override void CreateModifiers()
    {
        Modifier _modifier = new Modifier{type = ModifierType.TimeIncrease};
        Modifier _modifier2 = new Modifier{type = ModifierType.TimeMult};

        modifiers.Add(_modifier);
        modifiers.Add(_modifier2);
    }
}
