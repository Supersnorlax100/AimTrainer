using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    static ShopManager instance;

    // public ScriptableObject[] attatchmentPool;
    [SerializeField] Attatchment attatchment;

    [SerializeField] TMP_Text priceText;
    [SerializeField] TMP_Text attatchmentName;
    [SerializeField] TMP_Text statValue;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        attatchmentName.text = attatchment.name;
        statValue.text = attatchment.damage.ToString();
        priceText.text = attatchment.price.ToString();
    }
}
