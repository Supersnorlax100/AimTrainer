using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class RoomEvents : MonoBehaviour
{
    [SerializeField] REventUiScript rEventUiScript;
    public List<EventForRoom> avalableEvents = new List<EventForRoom>();
    public EventForRoom currentEvent;
    EventMods currentEventMods;

    public void TriggerOption(int optionNumber)
    {
        Cursor.lockState = CursorLockMode.None;
        int randomIndex = Random.Range(0, eventList.Count);
        currentEvent = eventList[randomIndex];
        GeneratEvent();
    }


    void GeneratEvent()
    {
        // Debug.Log("a");
        if (currentEvent != null)
        {
            eventText.text = currentEvent.eventText;
            if (currentEvent.backgroundImage != null)
            {
                eventImage.sprite = currentEvent.backgroundImage.sprite;
            }
            for (int i = 0; i < currentEvent.options.Length; i++)
            {
                // Debug.Log("b");
                if (i == 0)
                {
                    // Debug.Log("kahfd");
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

    async Task ApplyMod(EventModifier mod)
    {
        switch (mod.type)
        {
            case EventModTypes.STATS:
                if (mod.score)
                {
                    if (mod.posNeg)
                    {
                        GameManager.instance.score += GameManager.instance.score;
                    }
                    else
                    {
                        GameManager.instance.score -= GameManager.instance.score;
                        if (GameManager.instance.score < 0)
                        {
                            GameManager.instance.score = 0;
                        }
                    }
                }
                else
                {
                    if (mod.posNeg)
                    {
                        GameManager.instance.playerStats[(int)mod.modifierType] += mod.value;
                    }
                    else
                    {
                        gameObject.GetComponent<GameManager>().playerStats[(int)mod.modifierType] -= mod.value;
                        if (GameManager.instance.playerStats[(int)mod.modifierType] < 0)
                        {
                            GameManager.instance.playerStats[(int)mod.modifierType] = 0;
                        }
                    }
                }
                break;
            case EventModTypes.ITEMS:
                if (mod.givetake)
                {
                    if (mod.random)
                    {
                        Attachment item = GameManager.instance.CreateAttachment(mod.rarity, mod.itemType);
                    }
                    else
                    {
                        if (mod.item == null)
                        {
                            Attachment item = GameManager.instance.CreateAttachment(mod.rarity, mod.itemType);
                            if (InventoryManager.instance.CheckInventorySlots(item.type) == -1 && InventoryManager.instance.GetEquipedAttachment(item.type) != null)
                            {
                                Debug.Log("inventory full");
                                return;
                            }
                            InventoryManager.instance.EquipAttachment(item);

                        }
                        else
                        {
                            if (InventoryManager.instance.CheckInventorySlots(mod.item.type) == -1 && InventoryManager.instance.GetEquipedAttachment(mod.item.type) != null)
                            {
                                Debug.Log("inventory full");
                                return;
                            }
                            InventoryManager.instance.EquipAttachment(mod.item);
                        }
                    }
                }
                else
                {
                    if (mod.random)
                    {
                        int rand = Random.Range(1, System.Enum.GetValues(typeof(AttachmentType)).Length);
                        Attachment[] attachments = InventoryManager.instance.GetAttachments((AttachmentType)rand);  // TODO: Make Find Attachment func in InventoryManager and Get all Atchment of one Type and get All attachements of one Rarity, and get All Attachment . , and all thouse with out Equiped to
                        int ran2 = Random.Range(0, attachments.Length);
                        Attachment attachment = attachments[ran2];
                        // TODO: wait till payton finishes the first TODO 
                    }

                }
                break;
            case EventModTypes.OPTION:
                currentEvent.options[mod.optionIndex] = mod.optionText;
                currentEvent.eventMods[mod.optionIndex] = mod.modifiers;
                rEventUiScript.makeUi();
                break;
            case EventModTypes.EVENTTEXT:
                rEventUiScript.makeUi();
                currentEvent.eventText = mod.newEventText;
                break;
            case EventModTypes.EVENT:
                rEventUiScript.makeUi();
                currentEvent = mod.newEvent;
                break;
            case EventModTypes.SCENES:
                await SceneManager.LoadSceneAsync((int)mod.newScene);
                break;
            case EventModTypes.ENEMYMOD:
                if (mod.addTake)
                {
                    if (mod.perm)
                    {
                        foreach (MonoBehaviour eMod in GameManager.instance.modScripts)
                        {
                            if (eMod.GetType() == mod.mod.GetType())
                            {
                                foreach (MonoBehaviour eMod2 in GameManager.instance.modScriptsUnlocked)
                                {
                                    if (eMod2.GetType() == mod.mod.GetType())
                                    {
                                        foreach (MonoBehaviour eMod3 in GameManager.instance.modScriptsToLock)
                                        {
                                            if (eMod3.GetType() == mod.mod.GetType())
                                            {
                                                GameManager.instance.modScriptsToLock.Remove(eMod3);
                                                GameManager.instance.modScripts.Remove(eMod);
                                                return;
                                            }
                                            else
                                            {
                                                GameManager.instance.modScripts.Remove(eMod);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MonoBehaviour eMod3 in GameManager.instance.modScriptsToLock)
                                        {
                                            if (eMod3.GetType() == mod.mod.GetType())
                                            {
                                                GameManager.instance.modScriptsToLock.Remove(eMod3);
                                                GameManager.instance.modScriptsUnlocked.Add(eMod2);
                                                GameManager.instance.modScripts.Remove(eMod);
                                                return;
                                            }
                                            else
                                            {
                                                GameManager.instance.modScriptsUnlocked.Add(eMod);
                                                GameManager.instance.modScripts.Remove(eMod);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                foreach(MonoBehaviour eMod2 in GameManager.instance.modScriptsUnlocked)
                                {
                                    if (eMod2.GetType() == mod.mod.GetType())
                                    {
                                        foreach (MonoBehaviour eMod3 in GameManager.instance.modScriptsToLock)
                                        {
                                            if (eMod3.GetType() == mod.mod.GetType())
                                            {
                                                GameManager.instance.modScriptsToLock.Remove(eMod3);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (MonoBehaviour eMod3 in GameManager.instance.modScriptsToLock)
                                        {
                                            if (eMod3.GetType() == mod.mod.GetType())
                                            {
                                                GameManager.instance.modScriptsToLock.Remove(eMod3);
                                                GameManager.instance.modScriptsUnlocked.Add(mod.mod);
                                                return;
                                            }
                                            else
                                            {
                                                GameManager.instance.modScriptsUnlocked.Add(mod.mod);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        foreach (MonoBehaviour eMod in GameManager.instance.modScriptsUnlocked)
                        {
                            if (eMod.GetType() == mod.mod.GetType())
                            {
                                Debug.Log("mod already unlocked");
                                return;
                            }
                        }

                    }
                    else
                    {
                        // TODO: Make a way to trake number of floors they whent up befor taking it away
                    }
                }
                else
                {
                    if (mod.perm)
                    {

                    }
                    else
                    {
                        // Todo: same as above
                    }

                }
                break;
            case EventModTypes.TIME:
                // Ask bob how he wants this done.
                break;

        }
    }
}
