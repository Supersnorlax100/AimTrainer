using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopUpgradeScript : MonoBehaviour
{
    public Attachment attachment;
    
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text attachmentName;
    [SerializeField] TMP_Text statValue;

    [SerializeField] Image background;
    [SerializeField] Color commonColor;
    [SerializeField] Color unCommonColor;
    
    void Start()
    {
        attachmentName.text = attachment.name;
        // statValue.text = attachment.damage.ToString();
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
