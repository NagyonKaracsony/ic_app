using System;
using TMPro;
using UnityEngine;
namespace Assets
{
    public class GameTimeHandler : MonoBehaviour
    {
        private DateTime InGameTime = new DateTime(2200, 1, 1);
        public RefrenceHolder refrenceHolder;
        public static int InGameTimeScale = 0;
        private float timer = 0f;
        void Start()
        {
            refrenceHolder = FindObjectOfType<RefrenceHolder>();
            InGameTimeScale = 0;
            refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InGameTime.ToString("yyyy-MM-dd");
            refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Paused";
        }
        private void SecondaryUpdate()
        {
            InGameTime = InGameTime.AddDays(1);
            refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = InGameTime.ToString("yyyy-MM-dd");
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                InGameTimeScale = 0;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Paused";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                InGameTimeScale = 1;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "1x";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                InGameTimeScale = 2;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "2x";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                InGameTimeScale = 3;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "3x";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                InGameTimeScale = 4;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "4x";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                InGameTimeScale = 5;
                Time.timeScale = InGameTimeScale;
                refrenceHolder.MainCanvas.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "5x";
            }
            // Secondary update
            timer += Time.deltaTime;
            if (timer >= 1f)
            {
                timer = 0f;
                SecondaryUpdate();
            }
        }
    }
}