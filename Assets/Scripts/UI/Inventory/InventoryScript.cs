using UnityEngine;

public class InventoryScript : MonoBehaviour
{

    GameObject[][] attachmentSlots;

    GameObject[] equipedAttachmentSlots;
    public void EquipItem(Attachment attachment)
    {
        InventoryManager.instance.EquipAttachment(attachment);
    }

    public void UpdateUI()
    {
        
    }
}
