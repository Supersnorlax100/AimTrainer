using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Attachment attachment;
    [SerializeField] TMP_Text text;

    Image attImage;

    InventoryUIScript inventoryScript;

    public void EquipItem()
    {
        inventoryScript.EquipItem(attachment);
    }

    public void UpdateUI()
    {
        text.text = attachment.attachmentName; 
        attImage.sprite = InventoryManager.instance.attachmentImages[(int)attachment.type];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("on pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("on pointer exit");
    }
}
