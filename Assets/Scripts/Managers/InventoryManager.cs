using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public GameObject inventoryUi;

    public Attachment[] equipedAttachments;

    public Color[] rarityColors;
    // COMMON,
    // UNCOMMON,
    // RARE,
    // EPIC,
    // LEGENDARY

    public Sprite[] attachmentImages; // TODO: Prolly temp var
    

    Dictionary<AttachmentType, Attachment[]> inventory = new Dictionary<AttachmentType, Attachment[]>();

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
        for (int i = 0; i < Enum.GetNames(typeof(AttachmentType)).Length; i++)
        {
            inventory.Add((AttachmentType)i, new Attachment[]{null,null});
            Debug.Log("list length: " + inventory[(AttachmentType)i].Length);
        }
        equipedAttachments = new Attachment[Enum.GetNames(typeof(AttachmentType)).Length];
    }


    public void AddAttachment(Attachment attachment)
    {
        int freeSlot = CheckInventorySlots(attachment.type);
        Debug.Log("adding to inventory, slot: " + freeSlot);
        inventory[attachment.type][freeSlot] = attachment;
    }

    public void EquipAttachment(Attachment attachment)
    {
        Attachment curEquiped = equipedAttachments[(int)attachment.type];
        if (GetAttachmentSlot(attachment) == -1) // if being equiped from not an item slot (on purchase)
        {
            if (curEquiped)
            {
                AddAttachment(curEquiped); 
            }
            equipedAttachments[(int)attachment.type] = attachment;
            return;
        }
        Attachment toEquip = inventory[attachment.type][GetAttachmentSlot(attachment)];
        equipedAttachments[(int)attachment.type] = toEquip;
        if (curEquiped)
        {
            inventory[attachment.type][GetAttachmentSlot(attachment)] = curEquiped;
        }
    }

    public Attachment GetEquipedAttachment(AttachmentType attachmentType)
    {
        return equipedAttachments[(int)attachmentType];
    }

    public Attachment[] GetAttachments(AttachmentType attachmentType)
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
        for (int i = 0; i < inventory[attachment.type].Length; i++)
        {
            if (inventory[attachment.type][i] == attachment)
            {
                return i;
            }
        }
        return -1;
    }

    // if return -1 // Null
    // return 2 if equip is empty
    public int CheckInventorySlots(AttachmentType attachmentType)
    {
        // Debug.Log("count: " + inventory[attachmentType].Count);
        if (! inventory[attachmentType][0])
        {
            // Debug.Log("0 is free");
            return 0;
        }
        if (! inventory[attachmentType][1])
        {
            // Debug.Log("1 is free");
            return 1;
        }
        if (! equipedAttachments[(int)attachmentType])
        {
            // Debug.Log("equip is free");
            return 2;
        }
        // Debug.Log("no free slots");
        return -1;
    }

    public void DebugInventory()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[(AttachmentType)i][0])
            {
                Debug.Log((AttachmentType)i + " 0: " + inventory[(AttachmentType)i][0].id);
            }
            if (inventory[(AttachmentType)i][1])
            {
                Debug.Log((AttachmentType)i + " 1: " + inventory[(AttachmentType)i][1].id);
            }
        }
    }
}
