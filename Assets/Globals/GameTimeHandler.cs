using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Assets
{
    public class GameTimeHandler : MonoBehaviour
    {
        private DateTime InGameTime = new DateTime(2200, 01, 01);
        public static int InGameTimeScale = 0;
        public static int LastGameTimeScale = 1;
        public static bool IsLoading = false;
        private float tickTimer = 0f;
        public float baseTickInterval = 1f; // 1 second per tick at 1x speed
        public GameObject DateDisplay;
        public GameObject TimeScaleDisplay;
        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            IsLoading = false;
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            IsLoading = false;
            if (scene.name != "MainMenu")
            {
                DateDisplay = GameObject.Find("Counter");
                TimeScaleDisplay = GameObject.Find("CounterStatus");
            }
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        void Start()
        {
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
            if (InGameTimeScale == 0 || IsLoading) return;

            tickTimer += Time.deltaTime;
            if (tickTimer >= baseTickInterval)
            {
                tickTimer = 0f;
                MainTick();
            }
            SecondaryTick();
        }
        private void MainTick()
        {
            if (!DateDisplay.IsDestroyed())
            {
                InGameTime = InGameTime.AddDays(1);
                DateDisplay.GetComponent<TextMeshProUGUI>().text = InGameTime.ToString("yyyy-MM-dd");
                ShipHandler.HandleCombatTick();
            }
        }
        private void SecondaryTick()
        {

        }
    }
}