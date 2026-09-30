using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TargetType
{
    CLICKING,
    SWITCHING,
    TRACKING
}

public class EnemyScript : MonoBehaviour
{
    public TargetType targetType;

    public float health;
    public float scoreValue;
    public GameObject healthBarCanvas;
    private Slider healthBar;
    public bool canGetHit = true;
    List<MonoBehaviour> modScripts = new List<MonoBehaviour>();

    public void Start()
    {
        SetHealthBar();

        MonoBehaviour[] tempModArray = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour tempMod in tempModArray)
        {
            if (tempMod is EnemyScript)
            {
                continue;
            }
            modScripts.Add(tempMod);
        }
    }

    public void Update()
    {
        healthBarCanvas.gameObject.transform.position = transform.position;
    }
    public void GetHit(float damage, bool isCrit, float critMultiplier)
    {
        if (canGetHit)
        {
            UpdateHealthBar();
            if (isCrit) { health -= damage * critMultiplier; }
            else { health -= damage; }

            if (health <= 0)
            {
                Destroy(gameObject.transform.parent.gameObject);
                GameManager.instance.targetCount--;
                if (GetComponent<PetalScript>())
                {
                    EventHandeler.onPetalDeath?.Invoke();
                }
                else
                {
                    EventHandeler.onEnemyDeath?.Invoke();
                }
            }
            foreach (MonoBehaviour script in modScripts)
            {
                Debug.Log(script);
                script.Invoke("GetHit",0);
            }
        }
        else
        {
            return;
        }
    }

    public void UpdateHealthBar()
    {
        if (!healthBar.gameObject.activeSelf)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.value = health;
    }

    public void SetHealthBar()
    {
        healthBar = healthBarCanvas.GetComponentInChildren<Slider>();
        healthBar.maxValue = health;
        healthBar.value = health;
        healthBar.gameObject.SetActive(false);
    }
}
