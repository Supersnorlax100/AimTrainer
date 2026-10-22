using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// public class ItemBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
public class ItemBoxScript : MonoBehaviour
{
    [HideInInspector] public Attachment attachment;
    [SerializeField] TMP_Text text;

    InventoryScript inventoryScript;

    public void EquipItem()
    {
        inventoryScript.EquipItem(attachment);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("on pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("on pointer exit");
    }

    // OnPointerClick
}
