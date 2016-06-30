using UnityEngine;
using System.Collections;

namespace CirclesGame
{   
    [RequireComponent(typeof (CirclesSpawnerComponent))]
    public class GameControl : MonoBehaviour
    {
        public LevelDifficulty CurrentLevel {private set; get; }
        public int TotalScore { private set; get; }

        [SerializeField]
        private GameModel model;
        [SerializeField]
        private CirclesSpawnerComponent circlesSpawner;

        private int levelProgressScore;
        private int currentLevelNum;

        private void Start()
        {
            circlesSpawner.Model = model;

            InitLevel(0);

            NotificationCenter.Instance.AddObserver(this, OnScoreInc, NotificationName.ON_SCORE_INC);
        }

        private void IncScore(int increment)
        {
            TotalScore += increment;
            levelProgressScore += increment;
            NotificationCenter.Instance.PostNotification(this, NotificationName.NEW_TOTAL_SCORE, new NotificationArgsScores(TotalScore));
            TryToLevelUp();
        }

        public void TryToLevelUp()
        {
            if (levelProgressScore < CurrentLevel.PointsToComplete)
                return;

            if (currentLevelNum >= model.LevelsData.Length - 1)
                return;

            InitLevel(++currentLevelNum);
        }

        public void InitLevel(int num)
        {
            currentLevelNum = num;
            levelProgressScore = 0;
            CurrentLevel = model.LevelsData[currentLevelNum];
            model.CurrentLevel = CurrentLevel;
            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_NEW_LEVEl, new NotificationArgsNewLevel(CurrentLevel));
            Debug.Log("Level up " + currentLevelNum);
        }

        private void OnScoreInc(Notification notification)
        {
            int incScore = notification.GetArgs<NotificationArgsScores>().score;
            IncScore(incScore);
        }
    }
}

