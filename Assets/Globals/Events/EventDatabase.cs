using Assets;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class EventDatabase : MonoBehaviour
{
    public static Dictionary<string, GameEvent> Events = new();
    void Awake()
    {
        string json = File.ReadAllText(Application.dataPath + "../../Assets/Data/Events/Events.json");
        List<GameEvent> loadedEvents = JsonUtility.FromJson<Wrapper>(WrapArray(json)).events;

        foreach (var e in loadedEvents) Events[e.id] = e;
        Debug.Log("Loaded " + Events.Count + " events");
    }
    [System.Serializable]
    private class Wrapper
    {
        public List<GameEvent> events;
    }
    private string WrapArray(string rawJson)
    {
        return "{\"events\":" + rawJson + "}";
    }
}