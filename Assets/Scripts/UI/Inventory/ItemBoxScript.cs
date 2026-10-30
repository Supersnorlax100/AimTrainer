using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Attachment attachment;
    [SerializeField] GameObject attImageContainer;
    [SerializeField] Image attRarityImage;

    public InventoryUIScript inventoryScript;


    public void EquipItem()
    {
        if (attachment) inventoryScript.EquipAttachment(attachment);
        inventoryScript.HoverStats(attachment);
    }

    public void UpdateUI()
    {
        if (attImageContainer.GetComponentsInChildren<Transform>().Length > 1)
        {
            GameObject curAttImage = attImageContainer.GetComponentsInChildren<Transform>()[1].gameObject;
            Debug.Log("cur att image name: " + curAttImage.name);
            Destroy(curAttImage);
            attRarityImage = null;
        }
        if (! attachment) return;
        
        if (attachment.type == AttachmentType.Grip)
        {
            Instantiate(inventoryScript.gripImages[(int)attachment.rarity], attImageContainer.transform);
            return;   
        }
        GameObject _obj = Instantiate(inventoryScript.attachmentImages[(int)attachment.type], attImageContainer.transform);
        attRarityImage = _obj.GetComponentsInChildren<Image>()[1];
        attRarityImage.color = InventoryManager.instance.rarityColors[(int)attachment.rarity];
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
