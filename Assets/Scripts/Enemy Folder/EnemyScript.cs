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
    public bool respawns = true;
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
        AssignColor();
    }

    public void Update()
    {
        healthBarCanvas.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        if (!GetComponent<EnemyScript>().canGetHit)
        {
            GetComponent<Renderer>().material.color = Color.lightBlue;
        }
        else
        {
            AssignColor();
        }
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
                if (!respawns)
                {
                    EventHandeler.onSubEnemyDeath?.Invoke();
                }
                else
                {
                    EventHandeler.onEnemyDeath?.Invoke();
                }
            }
            foreach (MonoBehaviour script in modScripts)
            {
                Debug.Log(script);
                script?.Invoke("GetHit",0);
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

    private void AssignColor()
    {
        // Max RGB value is 255, Min is 0
        float b = 0;
        float g = 0;
        float r = 0;

        // if 9 enemy types 9/3 = 3 255/3 = 85
        if (GetComponent<BlinkyModScript>())
        {
            b = 85;
        }

        if (GetComponent<FlowerModScript>())
        {
            g = 85;
        }
        if (GetComponent<SubEnemyScript>())
        {
            r = 85;
        }
        // have to devide each value by 255
        Color color = new Color(r/255, g/255, b/255, 1);
        GetComponent<Renderer>().material.color = color;
    }
}
