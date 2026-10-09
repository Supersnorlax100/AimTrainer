using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Attatchment attatchment;
    [SerializeField] TMP_Text text;

    InventoryScript inventoryScript;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("on pointer enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("on pointer exit");
    }
}
