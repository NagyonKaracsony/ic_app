using System.Collections.Generic;
namespace Assets
{
    [System.Serializable]
    public class GameEvent
    {
        public string id;
        public string title;
        public string description;
        public List<EventOption> options;
    }
    [System.Serializable]
    public class EventOption
    {
        public string id;
        public string text;
        public List<Condition> conditions;
        public List<Effect> effects;
        public string nextEvent;
    }
    [System.Serializable]
    public class Condition
    {
        public string type;
        public int min;
    }
    [System.Serializable]
    public class Effect
    {
        public string type;
        public int value;
    }
}