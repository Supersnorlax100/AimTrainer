using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUpgradeScript : MonoBehaviour
{
    public Attachment attachment;
    
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text attachmentName;

    [SerializeField] Image background;
    [SerializeField] Color commonColor;
    [SerializeField] Color unCommonColor;
    
    [SerializeField] GameObject statContainer;
    [SerializeField] GameObject statBox;
    
    void Start()
    {
        foreach (Modifier mod in attachment.modifiers)
        {
            GameObject _statBox = Instantiate(statBox, statContainer.transform);
            // _statBox.GetComponent<StatBoxScript>().statName.text = mod.type.ToString();
            // _statBox.GetComponent<StatBoxScript>().statValue.text = mod.value.ToString();
            _statBox.GetComponent<StatBoxScript>().statName.text = "+" + mod.value.ToString() + " " + mod.type.ToString();
        }
        attachmentName.text = attachment.attachmentName;
        priceText.text = attachment.price.ToString();

        switch(attachment.rarity)
        {
            case Rarity.COMMON:
                background.color = commonColor;
                break;
            case Rarity.UNCOMMON:
                background.color = unCommonColor;
                break;
            default:
                background.color = Color.white;
                break;
        }
    }

    public void Purchase()
    {
        ShopManager.instance.Purchase(attachment);
    }
}
