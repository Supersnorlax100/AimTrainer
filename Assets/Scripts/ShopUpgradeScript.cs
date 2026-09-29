using UnityEngine;
using TMPro;

public class ShopUpgradeScript : MonoBehaviour
{
    [SerializeField] Attatchment attatchment;
    
    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text attatchmentName;
    [SerializeField] TMP_Text statValue;
    
    void Start()
    {
        attatchmentName.text = attatchment.name;
        statValue.text = attatchment.damage.ToString();
        priceText.text = attatchment.price.ToString();
    }

    public void Purchase()
    {
        ShopManager.instance.Purchase(attatchment);
    }
}
