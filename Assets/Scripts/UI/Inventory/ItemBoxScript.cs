using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Attachment attachment;
    [SerializeField] TMP_Text text;

    [SerializeField] Image attImage;

    [HideInInspector] public InventoryUIScript inventoryScript;

    public void EquipItem()
    {
        inventoryScript.EquipAttachment(attachment);
    }

    public void UpdateUI()
    {
        if (! attachment) return;
        text.text = attachment.attachmentName; 
        attImage.sprite = InventoryManager.instance.attachmentImages[(int)attachment.type];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("on pointer enter");
        if (attachment) inventoryScript.HoverStats(attachment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("on pointer exit");
        inventoryScript.HoverStats(null);
    }
}
