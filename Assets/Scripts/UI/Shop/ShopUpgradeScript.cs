using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUpgradeScript : MonoBehaviour
{
    public Attachment attachment;
    
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text attachmentName;

    [SerializeField] Image background;
    
    [SerializeField] GameObject statContainer;
    [SerializeField] GameObject statBox;
    
    void Start()
    {
        foreach (Modifier mod in attachment.modifiers)
        {
            GameObject _statBox = Instantiate(statBox, statContainer.transform);
            if (InventoryManager.instance.IsModTypePercentage(mod.type))
            {
                _statBox.GetComponent<StatBoxScript>().statName.text = "+" + mod.value.ToString() + "% " + mod.type.ToString();
            }
            else _statBox.GetComponent<StatBoxScript>().statName.text = "+" + mod.value.ToString() + " " + mod.type.ToString();
        }
        attachmentName.text = attachment.attachmentName;
        priceText.text = attachment.price.ToString();

        background.color = InventoryManager.instance.rarityColors[(int)attachment.rarity];
    }

    public void Purchase()
    {
        ShopManager.instance.Purchase(attachment);
    }
}
