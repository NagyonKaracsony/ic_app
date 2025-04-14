using Assets;
using System.Collections.Generic;
using UnityEngine;
public class EventManager : MonoBehaviour
{
    public GameEvent currentEvent;
    public void TriggerEvent(string eventId)
    {
        if (!EventDatabase.Events.TryGetValue(eventId, out currentEvent))
        {
            Debug.LogError("Event not found: " + eventId);
            return;
        }

        ShowEventUI(currentEvent);
    }
    public void ChooseOption(EventOption option)
    {
        ApplyEffects(option.effects);

        if (!string.IsNullOrEmpty(option.nextEvent)) TriggerEvent(option.nextEvent);
        else HideEventUI();
    }
    private void ApplyEffects(List<Effect> effects)
    {
        Civilization civilization = GameManager.playerCivilization;
        foreach (var effect in effects)
        {
            switch (effect.type)
            {
                case "addEnergy":
                    civilization.Energy += effect.value;
                    break;
                case "addMetal":
                    civilization.Metals += effect.value;
                    break;
                case "addFood":
                    civilization.Food += effect.value;
                    break;
                case "addAlloy":
                    civilization.Alloy += effect.value;
                    break;
                case "addConsumer":
                    civilization.Consumer += effect.value;
                    break;
            }
        }
    }
    private void ShowEventUI(GameEvent e)
    {
        foreach (var opt in e.options) if (CheckConditions(opt.conditions)) Debug.Log($"→ {opt.text}");
    }
    private bool CheckConditions(List<Condition> conditions)
    {
        if (conditions == null || conditions.Count == 0) return true;

        foreach (var condition in conditions)
        {
            switch (condition.type)
            {
                case "hasFleetPower":
                    int currentFleetPower = 1200; // Example value
                    if (currentFleetPower < condition.min) return false;
                    break;
                    // Add more conditions here
            }
        }

        return true;
    }
    private void HideEventUI()
    {
        Debug.Log("Event ended.");
    }
    void Start()
    {
        GetComponent<EventManager>().TriggerEvent("event.intro");
    }
}