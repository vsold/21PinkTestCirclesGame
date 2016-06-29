using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class GameModel
    {
        public int TotalScore { set; get; }
        private int levelProgressScore;
        private int currentLevelNum;

        public LevelDifficulty CurrentLevel { set; get; }
        public LevelDifficulty[] levelsData;

        public GameModel()
        {
            levelsData = new LevelDifficulty[3];
            levelsData[0] = new LevelDifficulty(new Vector2(150f, 400f), 150f, 5, new Vector2(1f, 2f), 100);
            levelsData[1] = new LevelDifficulty(new Vector2(140f, 350f), 200f, 10, new Vector2(0.5f, 1f), 200);
            levelsData[2] = new LevelDifficulty(new Vector2(100f, 300f), 250f, 20, new Vector2(0.2f, 0.5f), 500);

            InitLevel(0);
        }

        public void IncScore(int increment)
        {
            TotalScore += increment;
            levelProgressScore += increment;
        }

        public void TryToLevelUp()
        {
            if (levelProgressScore < CurrentLevel.PointsToComplete)
                return;

            if (currentLevelNum >= levelsData.Length - 1)
                return;

            InitLevel(currentLevelNum++);
        }

        public void InitLevel(int num)
        {
            currentLevelNum = num;
            levelProgressScore = 0;
            CurrentLevel = levelsData[currentLevelNum];
            NotificationCenter.Instance.PostNotification(null, NotificationName.ON_NEW_LEVEl, new NotificationArgsNewLevel(CurrentLevel));
            Debug.Log("Level up " + currentLevelNum);
        }

        public float GetRadius()
        {
            return Random.Range(CurrentLevel.RadiusMinMax.x, CurrentLevel.RadiusMinMax.y);
        }

        public float GetSpeed(float radius)
        {
            return CurrentLevel.BaseSpeed / radius * CurrentLevel.BaseSpeed;
        }

        public int GetScore(float radius)
        {
            return (int) (CurrentLevel.BaseScore/radius*CurrentLevel.BaseScore);
        }
    }

    public class LevelDifficulty
    {
        public Vector2 RadiusMinMax { private set; get; }
        public float BaseSpeed {private set; get; }
        public int BaseScore { private set; get; }
        public Vector2 SpawnTimeInterval { private set; get; }
        public int PointsToComplete { private set; get; }

        public LevelDifficulty(Vector2 radiusBounds, float baseSpeed, int baseScore, Vector2 spawnInterval, int pointsToComplete)
        {
            RadiusMinMax = radiusBounds;
            BaseSpeed = baseSpeed;
            BaseScore = baseScore;
            SpawnTimeInterval = spawnInterval;
            PointsToComplete = pointsToComplete;
        }
    }
}
