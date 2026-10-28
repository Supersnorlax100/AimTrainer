using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    [SerializeField] ItemBoxScript[] itemBoxes;

    [SerializeField] EquipedAttBoxScript[] equipedAttBoxes;
    ItemBoxScript[,] attachmentSlots = new ItemBoxScript[,]
    {{null, null},
    {null, null},
    {null, null},
    {null, null},
    {null, null},
    {null, null}};

    bool madeLists;

    void Start()
    {
        int j = 0;
        for (int i = 0; i < itemBoxes.Length; i++)
        {
            attachmentSlots[j,0] = itemBoxes[i];
            i++;
            attachmentSlots[j,1] = itemBoxes[i];
            j++;
        }
        madeLists = true;
    }

    void OnEnable()
    {
        if (madeLists) UpdateUI();
    }

    public void EquipAttachment(Attachment attachment)
    {
        InventoryManager.instance.EquipAttachment(attachment);
        UpdateUI();
    }

    public void UnequipAttachment(Attachment attachment)
    {
        InventoryManager.instance.UnequipAttachment(attachment);
        UpdateUI();
    }

    public void UpdateUI()
    {
        // EquipedUI
        for (int i = 0; i < Enum.GetNames(typeof(AttachmentType)).Length; i++)
        {
            equipedAttBoxes[i].attachment = InventoryManager.instance.equipedAttachments[i];
            equipedAttBoxes[i].inventoryScript = this;
            equipedAttBoxes[i].UpdateUI();
        }

        // Attachment Slots
        for (int i = 0; i < attachmentSlots?.GetLength(0); i++)
        {
            attachmentSlots[i, 0].attachment = InventoryManager.instance.GetAttachments((AttachmentType)i)[0];
            attachmentSlots[i, 1].attachment = InventoryManager.instance.GetAttachments((AttachmentType)i)[1];
        }
        foreach (ItemBoxScript itemBox in attachmentSlots)
        {
            itemBox.inventoryScript = this;
            itemBox.UpdateUI();
        }
    }

    public void UpdatePlayerStatsUI()
    {
        
    }
}
