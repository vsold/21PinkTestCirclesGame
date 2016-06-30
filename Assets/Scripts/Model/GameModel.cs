using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class GameModel : MonoBehaviour
    {
        public LevelDifficulty[] LevelsData { private set; get; }
        public LevelDifficulty CurrentLevel {set; get; }
        //Game data initialization
        void Awake()
        {
            LevelsData = new LevelDifficulty[3];
            LevelsData[0] = new LevelDifficulty(new Vector2(150f, 400f), 150f, 5, new Vector2(0.3f, 0.5f), 100);
            LevelsData[1] = new LevelDifficulty(new Vector2(140f, 350f), 180f, 10, new Vector2(0.2f, 0.4f), 200);
            LevelsData[2] = new LevelDifficulty(new Vector2(100f, 300f), 200f, 20, new Vector2(0.1f, 0.4f), 500);
        }
        // Game rules
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
            return (int)(CurrentLevel.RadiusMinMax.y / radius * CurrentLevel.BaseScore);
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
