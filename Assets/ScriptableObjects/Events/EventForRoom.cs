using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEditor;
public enum EventModTypes
{
    STATS, // modifierType enum insantiated in Attatchments scriptiable object.
    ITEMS,
    OPTION,
    EVENTTEXT,
    EVENT,
    SCENES,
    ENEMYMOD,
    TIME
    // SPECIAL SCENES
    // MOVE LOCATION
}
[Serializable]
public class EventModifier
{
    public EventModTypes type;
    #region STATS
    public bool score;
    public ModifierType modifierType;
    public float value;
    public bool posNeg = true;
    #endregion
    #region ITEMS
    public bool givetake = true;
    public AttachmentType itemType;
    public Rarity rarity;
    public bool random;
    public Attachment item;
    public bool randomRarity;
    public bool randomType;
    public bool RandomEquiped;
    #endregion
    #region OPTION
    public int optionIndex;
    public string optionText;
    public EventMods modifiers;
    public bool markedOut;// ToDo make this shit work.
    #endregion
    #region EVENTTEXT
    public string newEventText;
    #endregion
    #region EVENT
    public EventForRoom newEvent;
    #endregion
    #region SCENES
    public RoomType newScene;
    #endregion
    #region ENEMYMOD
    public bool addTake;
    public bool perm;
    public bool canBeFound;
    public MonoBehaviour mod;
    public int howManyFlors;
    #endregion
    #region TIME
    public float timeChange;
    public bool timeChangePosNeg;
    #endregion
}
[Serializable]
public class EventMods
{
    public List<EventModifier> eventModifier = new List<EventModifier>();
}

[CreateAssetMenu(fileName = "EventForRoom", menuName = "Scriptable Objects/EventForRoom")]
public class EventForRoom : ScriptableObject
{
    public Sprite backgroundImage;
    public string eventText;
    public List<string> options;
    public List<EventMods> eventMods = new List<EventMods>();
}

[CustomEditor(typeof(EventForRoom))]
public class eventForRoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (EventForRoom)target;
        script.backgroundImage = (Sprite)EditorGUILayout.ObjectField("Background Image", script.backgroundImage, typeof(Sprite), false);
        script.eventText = EditorGUILayout.TextField("Event Text", script.eventText);
        if (GUILayout.Button("add new option"))
        {
            script.options.Add("New Option");
            script.eventMods.Add(new EventMods());
        }
        for (int i = 0; i < script.options.Count; i++)
        {
            EditorGUILayout.LabelField($"Option {i + 1}");
            script.options[i] = EditorGUILayout.TextField("Option Text", script.options[i]);
            if (GUILayout.Button("Add modifier"))
            {
                script.eventMods[i].eventModifier.Add(new EventModifier());
            }
            Debug.Log(script.eventMods[i]);
            Debug.Log(script.eventMods[i].eventModifier.Count);
            for (int j = 0; j < script.eventMods[i].eventModifier.Count; j++)
            {
                EditorGUILayout.LabelField($"Modifier {j + 1}");
                var modifier = script.eventMods[i].eventModifier[j];
                modifier.type = (EventModTypes)EditorGUILayout.EnumPopup("Type", modifier.type);
                switch (modifier.type)
                {
                    case EventModTypes.STATS:
                        modifier.score = EditorGUILayout.Toggle("Changes Score?", modifier.score);
                        if (!modifier.score)
                        {
                            modifier.modifierType = (ModifierType)EditorGUILayout.EnumPopup("Modifier Type", modifier.modifierType);
                        }
                        modifier.value = EditorGUILayout.FloatField("Value", modifier.value);
                        modifier.posNeg = EditorGUILayout.Toggle("Positive/Negative", modifier.posNeg);
                        break;
                    case EventModTypes.ITEMS:
                        modifier.givetake = EditorGUILayout.Toggle("Give/Take", modifier.givetake);
                        if (modifier.givetake)
                        {
                            modifier.random = EditorGUILayout.Toggle("Random?", modifier.random);
                            if (modifier.random)
                            {
                                modifier.randomRarity = EditorGUILayout.Toggle("Random Rarity?", modifier.randomRarity);
                                if (!modifier.randomRarity)
                                {
                                    modifier.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", modifier.rarity);
                                }
                                modifier.randomType = EditorGUILayout.Toggle("Random Type?", modifier.randomType);
                                if (!modifier.randomType)
                                {
                                    modifier.itemType = (AttachmentType)EditorGUILayout.EnumPopup("Item Type", modifier.itemType);
                                }
                            }
                            else
                            {
                                modifier.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", modifier.rarity);
                                modifier.itemType = (AttachmentType)EditorGUILayout.EnumPopup("Item Type", modifier.itemType);
                                EditorGUILayout.LabelField("Item is if you want to make and pass a specific item, if not leave it blank and it will generate a random one based on the rarity and type");
                                modifier.item = (Attachment)EditorGUILayout.ObjectField("Item", modifier.item, typeof(Attachment), false);
                            }
                        }
                        else
                        {
                            modifier.random = EditorGUILayout.Toggle("Completly Random?", modifier.random);
                            if (!modifier.random)
                            {
                                modifier.randomRarity = EditorGUILayout.Toggle("Random Rarity?", modifier.randomRarity);
                                if (!modifier.randomRarity)
                                {
                                    modifier.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", modifier.rarity);
                                }
                                modifier.randomType = EditorGUILayout.Toggle("Random Type?", modifier.randomType);
                                if (!modifier.randomType)
                                {
                                    modifier.itemType = (AttachmentType)EditorGUILayout.EnumPopup("Item Type", modifier.itemType);
                                }
                                modifier.RandomEquiped = EditorGUILayout.Toggle("Random from Equiped?", modifier.RandomEquiped);
                            }
                        }
                        break;
                    case EventModTypes.OPTION:
                        modifier.optionIndex = EditorGUILayout.IntField("Option Index", modifier.optionIndex);
                        modifier.optionText = EditorGUILayout.TextField("Option Text", modifier.optionText);
                        if (modifier.modifiers == null)     modifier.modifiers = new EventMods();
                        if (modifier.modifiers.eventModifier == null)   modifier.modifiers.eventModifier = new List<EventModifier>();
                        EditorGUILayout.LabelField("Modifiers");
                        for (int k = 0; k < modifier.modifiers.eventModifier.Count; k++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"Modifier {k + 1}");
                            if (GUILayout.Button("Remove"))
                            {
                                modifier.modifiers.eventModifier.RemoveAt(k);
                                k--;
                                continue;
                            }
                            EditorGUILayout.EndHorizontal();
                            var modifierModifiers = modifier.modifiers.eventModifier[k];
                            modifierModifiers.type = (EventModTypes)EditorGUILayout.EnumPopup("Type", modifier.type);
                            // You can add more fields here based on the type of modifier
                        }
                        if (GUILayout.Button("Add Modifier"))
                        {
                            modifier.modifiers.eventModifier.Add(new EventModifier());
                        }
                        break;
                    case EventModTypes.EVENTTEXT:
                        modifier.newEventText = EditorGUILayout.TextField("New Event Text", modifier.newEventText);
                        break;
                    case EventModTypes.EVENT:
                        modifier.newEvent = (EventForRoom)EditorGUILayout.ObjectField("New Event", modifier.newEvent, typeof(EventForRoom), false);
                        break;
                    case EventModTypes.SCENES:
                        modifier.newScene = (RoomType)EditorGUILayout.EnumPopup("New Scene", modifier.newScene);
                        break;
                    case EventModTypes.ENEMYMOD:
                        modifier.canBeFound = EditorGUILayout.Toggle("Can be found Now?", modifier.canBeFound);
                        if (!modifier.canBeFound)
                        {
                            modifier.addTake = EditorGUILayout.Toggle("Add/Take", modifier.addTake);
                            modifier.perm = EditorGUILayout.Toggle("Permanently added or Taken", modifier.perm);
                            if (!modifier.perm)
                            {
                                modifier.howManyFlors = EditorGUILayout.IntField("How many floors? warning this is not added yet", modifier.howManyFlors); // overscop for now
                            }
                            modifier.mod = (MonoBehaviour)EditorGUILayout.ObjectField("Modifier", modifier.mod, typeof(MonoBehaviour), false);
                        }
                        break;
                    case EventModTypes.TIME:
                        modifier.timeChange = EditorGUILayout.FloatField("Time Change Amount", modifier.timeChange);
                        modifier.timeChangePosNeg = EditorGUILayout.Toggle("Positive/Negative", modifier.timeChangePosNeg);
                        break;
                    default:
                        break;
                }

                if (GUILayout.Button("Remove Modifier"))
                {
                    script.eventMods[i].eventModifier.RemoveAt(j);
                    j--;
                    continue;
                }
            }

            if (GUILayout.Button("Remove Option"))
            {
                script.options.RemoveAt(i);
                script.eventMods.RemoveAt(i);
            }

        }
    }
}


//[CustomEditor(typeof(EventModifier))]
//public class EventModifierEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        var script = (EventModifier)target;
//        switch (script.type)
//        {
//            case EventModTypes.STATS:
//                script.modifierType = (ModifierType)EditorGUILayout.EnumPopup("Modifier Type", script.modifierType);
//                script.value = EditorGUILayout.FloatField("Value", script.value);
//                script.posNeg = EditorGUILayout.Toggle("Positive/Negative", script.posNeg);
//                break;
//            case EventModTypes.ITEMS:
//                script.givetake = EditorGUILayout.Toggle("Give/Take", script.givetake);
//                if (script.givetake)
//                {
//                    script.random = EditorGUILayout.Toggle("Random?", script.random);
//                    if (script.random)
//                    {
//                        script.randomRarity = EditorGUILayout.Toggle("Random Rarity?", script.randomRarity);
//                        if (!script.randomRarity)
//                        {
//                            script.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", script.rarity);
//                        }
//                        script.randomType = EditorGUILayout.Toggle("Random Type?", script.randomType);
//                        if (!script.randomType)
//                        {
//                            script.itemType = (AttachmentType)EditorGUILayout.EnumPopup("Item Type", script.itemType);
//                        }
//                    }
//                    else
//                    {
//                        script.item = (Attachment)EditorGUILayout.ObjectField("Item", script.item, typeof(Attachment), false);
//                    }
//                }
//                else
//                {
//                    script.random = EditorGUILayout.Toggle("Completly Random?", script.random);
//                    if (!script.random)
//                    {
//                        script.randomRarity = EditorGUILayout.Toggle("Random Rarity?", script.randomRarity);
//                        if (!script.randomRarity)
//                        {
//                            script.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", script.rarity);
//                        }
//                        script.randomType = EditorGUILayout.Toggle("Random Type?", script.randomType);
//                        if (!script.randomType)
//                        {
//                            script.itemType = (AttachmentType)EditorGUILayout.EnumPopup("Item Type", script.itemType);
//                        }
//                        script.RandomEquiped = EditorGUILayout.Toggle("Random from Equiped?", script.RandomEquiped);
//                    }
//                }
//                break;
//            case EventModTypes.OPTION:
//                script.optionIndex = EditorGUILayout.IntField("Option Index", script.optionIndex);
//                script.optionText = EditorGUILayout.TextField("Option Text", script.optionText);
//                if (script.modifiers == null)
//                    script.modifiers = new EventMods();
//                if (script.modifiers.eventModifier == null)
//                    script.modifiers.eventModifier = new List<EventModifier>();
//                EditorGUILayout.LabelField("Modifiers");
//                for (int i = 0; i < script.modifiers.eventModifier.Count; i++)
//                {
//                    EditorGUILayout.BeginHorizontal();
//                    EditorGUILayout.LabelField($"Modifier {i + 1}");
//                    if (GUILayout.Button("Remove"))
//                    {
//                        script.modifiers.eventModifier.RemoveAt(i);
//                        i--;
//                        continue;
//                    }
//                    EditorGUILayout.EndHorizontal();
//                    var modifier = script.modifiers.eventModifier[i];
//                    modifier.type = (EventModTypes)EditorGUILayout.EnumPopup("Type", modifier.type);
//                    // You can add more fields here based on the type of modifier
//                }
//                if (GUILayout.Button("Add Modifier"))
//                {
//                    script.modifiers.eventModifier.Add(new EventModifier());
//                }
//                break;
//            case EventModTypes.EVENTTEXT:
//                script.newEventText = EditorGUILayout.TextField("New Event Text", script.newEventText);
//                break;
//            case EventModTypes.EVENT:
//                script.newEvent = (EventForRoom)EditorGUILayout.ObjectField("New Event", script.newEvent, typeof(EventForRoom), false);
//                break;
//        }
//    }
//}