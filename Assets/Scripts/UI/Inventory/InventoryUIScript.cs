using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIScript : MonoBehaviour
{
    bool madeStartStuff;

    EquipedAttBoxScript[] equipedAttBoxes;
    ItemBoxScript[,] attachmentSlots = new ItemBoxScript[,]
    {{null, null},
    {null, null},
    {null, null},
    {null, null},
    {null, null},
    {null, null}};

    // [SerializeField] GameObject[] equipedAttachmentSlots;

    // [SerializeField] Image[] equipedAttSlotsRarityImg;

    // void Awake()
    // {
    //     EventHandeler.purchase += UpdateUI;
    // }

    void Start()
    {
        ItemBoxScript[] _itemBoxes = GetComponentsInChildren<ItemBoxScript>();
        // Debug.Log("item boxes length: " + _itemBoxes.Length);
        int j = 0;
        for (int i = 0; i < _itemBoxes.Length; i++)
        {
            // Debug.Log("att slot 0," + j + " = itemboxes " + i);
            attachmentSlots[j,0] = _itemBoxes[i];
            i++;
            // Debug.Log("att slot 1," + j + " = itemboxes " + i);
            attachmentSlots[j,1] = _itemBoxes[i];
            j++;
        }
        Debug.Log("start is prolly find");
        madeStartStuff = true;
    }

    void OnEnable()
    {
        if (madeStartStuff) UpdateUI();
        else
        {
            Debug.Log("not made");
        }
    }

    public void EquipItem(Attachment attachment)
    {
        InventoryManager.instance.EquipAttachment(attachment);
        UpdateUI();
    }


    public void UpdateUI()
    {
        Debug.Log("updateUI");
        // EquipedUI
        for (int i = 0; i < Enum.GetNames(typeof(AttachmentType)).Length; i++)
        {
            equipedAttBoxes[i].inventoryScript = this;
            equipedAttBoxes[i].UpdateUI();
        }
        // for (int i = 0; i < equipedAttachmentSlots.Length; i++)
        // {
        //     if (InventoryManager.instance.equipedAttachments[i])
        //     {
        //         equipedAttSlotsRarityImg[i].enabled = true;
        //         equipedAttSlotsRarityImg[i].color = InventoryManager.instance.rarityColors[(int)InventoryManager.instance.equipedAttachments[i].rarity];
        //     }
        //     else
        //     {
        //         equipedAttSlotsRarityImg[i].enabled = false;
        //     }
        // }
        // Attachment Slots
        Debug.Log("att slots length: " + attachmentSlots.GetLength(2));
        for (int i = 0; i < attachmentSlots.GetLength(2); i++)
        {
            Debug.Log("run, i: " + i);
            attachmentSlots[0, i].attachment = InventoryManager.instance.GetAttachments((AttachmentType)i)[0];
            attachmentSlots[1, i].attachment = InventoryManager.instance.GetAttachments((AttachmentType)i)[1];
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
