using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "Scriptable Objects/Attatchment/Trigger")]
public class Trigger : Attachment
{
    void Awake()
    {
        attachmentName = nameof(AttachmentType.Trigger);

        if (modifiers.Count == 0)
        {
            CreateModifiers();
        }
    }

    public override void CreateModifiers()
    {
        Modifier lazerMod = new Modifier{type = ModifierType.ComboIncrease};
        modifiers.Add(lazerMod);
    }
}
