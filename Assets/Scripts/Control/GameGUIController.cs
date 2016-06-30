using UnityEngine;
using UnityEngine.UI;

namespace CirclesGame
{
    public class GameGUIController : MonoBehaviour
    {
        [SerializeField]
        private Text scoreText;

        void Start() 
        {
            NotificationCenter.Instance.AddObserver(this, OnScoreInc, NotificationName.NEW_TOTAL_SCORE);
	    }

        private void OnScoreInc(Notification notification)
        {
            var args = notification.GetArgs<NotificationArgsScores>();
            if (args != null)
                scoreText.text = args.score.ToString();
        }
    }
}

