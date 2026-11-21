using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class REventUiScript : MonoBehaviour
{
    public Image backGroundImage;
    public TMP_Text eventText;
    public GameObject eventButtonPrefab;
    [SerializeField] GameObject layoutGroup;
    [SerializeField] RoomEvents roomEvents;
    public List<Button> eventButtons = new List<Button>();


    private void Start()
    {
        makeUi();
    }

    public void makeUi()
    {
        eventText.text = roomEvents.currentEvent.eventText;
        eventButtons.Clear();
        backGroundImage.sprite = roomEvents.currentEvent.backgroundImage;
        makeOptionsAndAssingLayout();
    }

    private void makeOptionsAndAssingLayout()
    {
        int numberOfOptions = roomEvents.currentEvent.options.Count;
        GridLayoutGroup layoutGroupComponent = layoutGroup.GetComponent<GridLayoutGroup>();
        switch (numberOfOptions)
        {
            case 2:
                layoutGroupComponent.constraint = GridLayoutGroup.Constraint.Flexible;
                layoutGroupComponent.cellSize = new Vector2(540, 300);
                break;
            case 3:
                layoutGroupComponent.constraint = GridLayoutGroup.Constraint.FixedRowCount;
                layoutGroupComponent.constraintCount = 3;
                layoutGroupComponent.cellSize = new Vector2(1080, 100);
                break;
            case 4:
                layoutGroupComponent.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                layoutGroupComponent.constraintCount = 2;
                layoutGroupComponent.cellSize = new Vector2(540, 150);
                break;
            default:
                Debug.LogError(" not the correct nummber of options for events");
                break;
        }
        for(int i =0; i < numberOfOptions; i++)
        {
            GameObject button = Instantiate(eventButtonPrefab, layoutGroup.transform);
            button.GetComponentInChildren<TMP_Text>().text = roomEvents.currentEvent.options[i];
            switch(i+1)
            {
                case 1:
                    button.GetComponent<Button>().onClick.AddListener(Option1Pressed);
                    button.GetComponent<TextMeshProUGUI>().text = roomEvents.currentEvent.options[i];
                    break;
                case 2:
                    button.GetComponent<Button>().onClick.AddListener(Option2Pressed);
                    button.GetComponent<TextMeshProUGUI>().text = roomEvents.currentEvent.options[i];
                    break;
                case 3:
                    button.GetComponent<Button>().onClick.AddListener(Option3Pressed);
                    button.GetComponent<TextMeshProUGUI>().text = roomEvents.currentEvent.options[i];
                    break;
                case 4:
                    button.GetComponent<Button>().onClick.AddListener(Option4Pressed);
                    button.GetComponent<TextMeshProUGUI>().text = roomEvents.currentEvent.options[i];
                    break;  
                default:
                    Debug.LogError(" not the correct nummber of options for events");
                    break;
            }
        }
    }
    public void Option1Pressed()
    {
        roomEvents.TriggerOption(1);
    }
    public void Option2Pressed()
    {
        roomEvents.TriggerOption(2);
    }
    public void Option3Pressed()
    {
        roomEvents.TriggerOption(3);
    }
    public void Option4Pressed()
    {
        roomEvents.TriggerOption(4);
    }
}
