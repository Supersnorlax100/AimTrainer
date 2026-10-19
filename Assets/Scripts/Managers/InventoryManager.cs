using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public GameObject inventoryUi;

    Dictionary<AttachmentType, Attachment> equipedAttachments = new Dictionary<AttachmentType, Attachment>();

    Dictionary<AttachmentType, List<Attachment>> inventory = new Dictionary<AttachmentType, List<Attachment>>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }    
        else
        {
            Destroy(gameObject);
        }
    }


    public void AddAttachment(Attachment attachment)
    {
        if (attachment = null)
        {
            return;
        }

        int freeSlot = CheckInventorySlots(attachment.type);
        if (freeSlot == 0)
        {
            Debug.Log("inventory is full");
            return;
        }
        Debug.Log("adding to inventory");
        inventory[attachment.type][freeSlot-1] = attachment;
    }

    public void EquipAttachment(Attachment attachment)
    {
        Attachment curEquiped = equipedAttachments[attachment.type];
        if (GetAttachmentSlot(attachment) == -1) // cuz attachments are auto equip on purchase
        {
            AddAttachment(curEquiped);
            equipedAttachments[curEquiped.type] = attachment;
            return;
        }
        Attachment toEquip = inventory[attachment.type][GetAttachmentSlot(attachment)];
        inventory[attachment.type][GetAttachmentSlot(attachment)] = curEquiped;
        equipedAttachments[attachment.type] = toEquip;
    }

    public Attachment GetEquipedAttachment(AttachmentType attachmentType)
    {
        return equipedAttachments[attachmentType];
    }

    public List<Attachment> GetAttachments(AttachmentType attachmentType)
    {
        return inventory[attachmentType];
    }

    public Attachment GetAttachment(Attachment attachment)
    {
        foreach (Attachment _attachment in inventory[attachment.type])
        {
            if (_attachment == attachment)
            {
                return _attachment;
            }
        }
        return null;
    }

    public int GetAttachmentSlot(Attachment attachment)
    {
        for (int i = 0; i < inventory[attachment.type].Count; i++)
        {
            if (inventory[attachment.type][i] == attachment)
            {
                return i;
            }
        }
        return -1;
    }

    // if return -1 // Null
    public int CheckInventorySlots(AttachmentType attachmentType)
    {
        if (inventory[attachmentType][0] == null)
        {
            Debug.Log("0 is free");
            return 0;
        }
        if (inventory[attachmentType][1] == null)
        {
            Debug.Log("1 is free");
            return 1;
        }
        Debug.Log("no free slots");
        return -1;
    }
}
