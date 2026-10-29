using System;
using System.Threading.Tasks;
using TMPro;
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

    [SerializeField] TMP_Text attachmentName; // att stats att name
    [SerializeField] TMP_Text[] hoveredAttStats;
    [SerializeField] TMP_Text[] equipedAttStats;

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

    public void HoverStats(Attachment attachment)
    {
        // Will hide if null
        if (attachment == null)
        {
            foreach (TMP_Text text in hoveredAttStats)
            {
                text.text = "";
            }    
            foreach (TMP_Text text in equipedAttStats)
            {
                text.text = "";
            }
            return;
        }

        attachmentName.text = attachment.attachmentName;
        for (int i = 0; i < attachment.modifiers.Count; i++)
        {
            Debug.Log("i: " + i );
            Modifier curMod = attachment.modifiers[i];
            if (attachment.modifiers[i] == null) 
            {
                hoveredAttStats[i].text = "";   
                return;
            }
            hoveredAttStats[i].text = "+" + curMod.value.ToString() + " " + curMod.type.ToString();
        }
        for (int i = 0; i < InventoryManager.instance.equipedAttachments[(int)attachment.type].modifiers.Count; i++)
        {
            Attachment equipedAtt = InventoryManager.instance.equipedAttachments[(int)attachment.type];
            Modifier curMod = equipedAtt.modifiers[i];
            if (equipedAtt == null || curMod == null) 
            {
                equipedAttStats[i].text = "";
                return;
            }
            // equipedAttStats[i].text = "+" + curMod.value.ToString() + " " + curMod.type.ToString();
            // These ifs add the + or - green/red for the att that is being hovered compared to the equiped
            if (attachment.modifiers[i].value == curMod.value)
            {
                return;
            }
            if (attachment.modifiers[i].value > curMod.value)
            {
                equipedAttStats[i].text = "+" + (Mathf.Round((attachment.modifiers[i].value - curMod.value) * 100)/100).ToString();
                equipedAttStats[i].color = Color.green;   
            }
            else 
            {
                equipedAttStats[i].text = "-" + (Math.Round((curMod.value - attachment.modifiers[i].value) * 100)/100).ToString();
                equipedAttStats[i].color = Color.red; 
            }
        }
    }

    public void UpdatePlayerStatsUI()
    {
        
    }
}
