using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace CirclesGame
{
    public class LevelGUITimer : MonoBehaviour
    {
        [SerializeField] 
        private Text timerText;

        public string TimeString
        {
            get { return timerText.text; }
        }

        private const float updateInterval = 1;

        public void Start()
        {
            Stop();
            StartCoroutine(TimerCoroutine());
        }

        public void Stop()
        {
            StopAllCoroutines();
        }

        private IEnumerator TimerCoroutine()
        {
            float startTime = Time.timeSinceLevelLoad;

            while (true)
            {
                float timeDiff = Time.timeSinceLevelLoad - startTime;
                var time = TimeSpan.FromSeconds(timeDiff);
                timerText.text = string.Format("{0:00}:{1:00}", time.Minutes, time.Seconds);
                yield return new WaitForSeconds(updateInterval);
            }
        }
    }
}