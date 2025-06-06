﻿using Assets;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class EventDatabase : MonoBehaviour
{
    public static Dictionary<string, GameEvent> Events = new();
    void Awake()
    {
        string json = File.ReadAllText(Path.Combine(Application.streamingAssetsPath + "\\Events\\Events.json"));
        List<GameEvent> loadedEvents = JsonUtility.FromJson<Wrapper>(WrapArray(json)).events;
        foreach (var e in loadedEvents) Events[e.id] = e;
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