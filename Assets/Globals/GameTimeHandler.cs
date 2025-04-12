using System;
using TMPro;
using UnityEngine;
namespace Assets
{
    public class GameTimeHandler : MonoBehaviour
    {
        private DateTime InGameTime = new DateTime(2200, 01, 01);
        public RefrenceHolder refrenceHolder;
        public static int InGameTimeScale = 0;
        public static int LastGameTimeScale = 1;
        private float tickTimer = 0f;
        public float baseTickInterval = 1f; // 1 second per tick at 1x speed
        public GameObject DateDisplay;
        public GameObject TimeScaleDisplay;
        void Start()
        {
            refrenceHolder = FindObjectOfType<RefrenceHolder>();
            InGameTimeScale = 0;
            DateDisplay.GetComponent<TextMeshProUGUI>().text = InGameTime.ToString("yyyy-MM-dd");
            TimeScaleDisplay.GetComponent<TextMeshProUGUI>().text = "Paused";
        }
        public void SetInGameTime(int timeIndex)
        {
            if (timeIndex == 0 && InGameTimeScale != 0) LastGameTimeScale = InGameTimeScale;
            InGameTimeScale = timeIndex;
            Time.timeScale = InGameTimeScale;
            if (timeIndex != 0) TimeScaleDisplay.GetComponent<TextMeshProUGUI>().text = $"{timeIndex}x";
            else TimeScaleDisplay.GetComponent<TextMeshProUGUI>().text = "Paused";
        }
        public void SetLastGameTime()
        {
            Time.timeScale = LastGameTimeScale;
            TimeScaleDisplay.GetComponent<TextMeshProUGUI>().text = $"{LastGameTimeScale}x";
        }
        void Update()
        {
            float adjustedInterval = baseTickInterval / InGameTimeScale;
            if (InGameTimeScale == 0f) return; // paused
            else
            {
                tickTimer += Time.deltaTime;
                if (tickTimer >= adjustedInterval)
                {
                    tickTimer = 0f;
                    Tick();
                }
            }
        }
        private void Tick()
        {
            InGameTime = InGameTime.AddDays(1);
            DateDisplay.GetComponent<TextMeshProUGUI>().text = InGameTime.ToString("yyyy-MM-dd");
        }
    }
}