using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class RoomEvents : MonoBehaviour
{
    [SerializeField] TMP_Text eventText;

    [SerializeField] Button option1;
    [SerializeField] Button option2;
    [SerializeField] Button option3;

    [SerializeField] Image eventImage;

    [SerializeField] EventForRoom currentEvent;
    [SerializeField] List<EventForRoom> eventList;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        int randomIndex = Random.Range(0, eventList.Count);
        currentEvent = eventList[randomIndex];
        GeneratEvent();
    }


    void GeneratEvent()
    {
        Debug.Log("a");
        if (currentEvent != null)
        {
            eventText.text = currentEvent.eventText;
            if (currentEvent.backgroundImage != null)
            {
                eventImage.sprite = currentEvent.backgroundImage.sprite;
            }
            for (int i = 0; i < currentEvent.optionCount; i++)
            {
                Debug.Log("b");
                if (i == 0)
                {
                    Debug.Log("kahfd");
                    option1.gameObject.SetActive(true);
                    option1.GetComponentInChildren<TMP_Text>().text = currentEvent.options[i];
                }
                else if (i == 1)
                {
                    option2.gameObject.SetActive(true);
                    option2.GetComponentInChildren<TMP_Text>().text = currentEvent.options[i];
                }
                else if (i == 2)
                {
                    option3.gameObject.SetActive(true);
                    option3.GetComponentInChildren<TMP_Text>().text = currentEvent.options[i];
                }
            }
        }
    }


    public void Option1Selected()
    {
        ApliyModifiers(currentEvent.eventmodifiersO1);
    }
    public void Option2Selected()
    {
        ApliyModifiers(currentEvent.eventmodifiersO2);
    }
    public void Option3Selected()
    {
        ApliyModifiers (currentEvent.eventmodifiersO3);
    }

    void ApliyModifiers(List<EventModifier> modifiers)
    {
        foreach (EventModifier modifier in modifiers)
        {
            switch (modifier.type)
            {
                case EventModTypes.Score:
                    GameManager.instance.score += (int)modifier.value;
                    break;
                case EventModTypes.Damage:
                    GameManager.instance.damageIncrease += modifier.value;
                    break;
                case EventModTypes.DamageMult:
                    GameManager.instance.damageMult += modifier.value;
                    break;
                case EventModTypes.CritMult:
                    GameManager.instance.critMult += modifier.value;
                    break;
                case EventModTypes.CritSize:
                    GameManager.instance.critSize += modifier.value;
                    break;
                case EventModTypes.BulletSize:
                    // toDo: Implement bullet size modification logic
                    break;
                case EventModTypes.TimeIncrease:
                    GameManager.instance.bonusTime += (int)modifier.value;
                    break;
                case EventModTypes.TimeMult:
                    GameManager.instance.timeMult += modifier.value;
                    break;
                case EventModTypes.ComboTime:
                    GameManager.instance.maxComboTimer += modifier.value;
                    break;
                case EventModTypes.ComboIncrease:
                    GameManager.instance.comboIncrease += modifier.value;
                    break;
                case EventModTypes.ComboMult:
                    GameManager.instance.comboMultAddition += modifier.value;
                    break;
            }
        }
        EventHandeler.enterRoom?.Invoke();
    }
}
