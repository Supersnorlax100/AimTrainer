using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipedAttBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Attachment attachment;
    [SerializeField] TMP_Text text;

    [SerializeField] Image attImage;

    [HideInInspector] public InventoryUIScript inventoryScript;


    public void UnequipItem()
    {
        // inventoryScript.EquipItem(attachment);
        Debug.Log("unequip");
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
