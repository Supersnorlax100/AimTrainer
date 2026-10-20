using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public GameObject inventoryUi;

    public Attachment[] equipedAttachments;

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

        // Debug.Log(inventory.Count);

        // inventory.Add(AttachmentType.Lazer, new List<Attachment>(2));

        // Debug.Log(inventory.Count);

        for (int i = 0; i < Enum.GetNames(typeof(AttachmentType)).Length; i++)
        {
            inventory[(AttachmentType)i] = new List<Attachment>(2);
            inventory[(AttachmentType)i].Append(GameManager.instance.attachmentPool[0]);
            Debug.Log("list length: " + inventory[(AttachmentType)i].Count);
        }
        equipedAttachments = new Attachment[Enum.GetNames(typeof(AttachmentType)).Length];
    }


    public void AddAttachment(Attachment attachment)
    {
        Debug.Log("attach id: " + attachment.id);
        Debug.Log("attach type: " + attachment.type);
        int freeSlot = CheckInventorySlots(attachment.type);
        if (freeSlot == -1)
        {
            Debug.Log("inventory is full");
            return;
        }
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
    // return 2 if equip is empty
    public int CheckInventorySlots(AttachmentType attachmentType)
    {
        // Debug.Log("count: " + inventory[attachmentType].Count);
        if (! inventory[attachmentType][0])
        {
            Debug.Log("0 is free");
            return 0;
        }
        // if (inventory[attachmentType].Count == 0 || inventory[attachmentType][0] == null)
        // {
        //     Debug.Log("0 is free");
        //     return 0;
        // }
        if (! inventory[attachmentType][1])
        {
            Debug.Log("1 is free");
            return 1;
        }
        if (! equipedAttachments[(int)attachmentType])
        {
            Debug.Log("equip is free");
            return 2;
        }
        Debug.Log("no free slots");
        return -1;
    }
}
