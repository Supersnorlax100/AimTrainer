using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    // public ScriptableObject[] attatchmentPool;
    [SerializeField] TMP_Text scoreText;

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
        EventHandeler.updateUI += UpdateUI;
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + GameManager.instance.score.ToString();
    }

    public void Purchase(Attatchment attatchment)
    {
        if (GameManager.instance.score < (int)Mathf.Round(attatchment.price))
        {
            Debug.Log("brokie");
            return;
        }
        GameManager.instance.score -= (int)Mathf.Round(attatchment.price);
        EventHandeler.updateUI?.Invoke();
    }
}
