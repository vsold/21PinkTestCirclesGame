using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CirclesGame
{
    public interface IGameModel
    {
        CircleSizes GetSize();
        CircleColors GetColor();
        float GetSpeed(CircleSizes size);
        int GetScore(CircleSizes size);
    }

    public class GameModel : MonoBehaviour, IGameModel
    {
        public const int SIZE_BASE = 32;
        public LevelDifficulty[] LevelsData { private set; get; }
        public LevelDifficulty CurrentLevel {set; get; }
        //Game data initialization
        void Awake()
        {
            LevelsData = new LevelDifficulty[5];
            LevelsData[0] = new LevelDifficulty(40f, 15, new Vector2(0.8f, 1.2f), 50);
            LevelsData[1] = new LevelDifficulty(80f, 30, new Vector2(0.4f, 0.6f), 100);
            LevelsData[2] = new LevelDifficulty(100f, 35, new Vector2(0.3f, 0.4f), 100);
            LevelsData[3] = new LevelDifficulty(120f, 35, new Vector2(0.2f, 0.3f), 100);
            LevelsData[4] = new LevelDifficulty(140f, 35, new Vector2(0.1f, 0.2f), 100);
        }
        // Game rules
        public CircleSizes GetSize()
        {
            return GetRandomEnumValue<CircleSizes>();
        }

        public CircleColors GetColor()
        {
            return GetRandomEnumValue<CircleColors>();
        }

        public float GetSpeed(CircleSizes size)
        {
            return CurrentLevel.BaseSpeed / ((int)size * SIZE_BASE) * CurrentLevel.BaseSpeed;
        }

        public int GetScore(CircleSizes size)
        {
            return (int)(1f / (float)size * CurrentLevel.BaseScore);
        }

        public T GetRandomEnumValue<T>()
        {
            var array = Enum.GetValues(typeof(T));
            var randValue = (T)array.GetValue(Random.Range(0, array.Length));
            return randValue;
        }
    }

    public class LevelDifficulty
    {
        public CircleSizes Size { private set; get; }
        public float BaseSpeed {private set; get; }
        public int BaseScore { private set; get; }
        public Vector2 SpawnTimeInterval { private set; get; }
        public int PointsToComplete { private set; get; }

        public LevelDifficulty(float baseSpeed, int baseScore, Vector2 spawnInterval, int pointsToComplete)
        {
            BaseSpeed = baseSpeed;
            BaseScore = baseScore;
            SpawnTimeInterval = spawnInterval;
            PointsToComplete = pointsToComplete;
        }
    }

    public enum CircleSizes
    {
        Tiny = 1,
        Small = 2,
        Normal = 4,
        Large = 8
    }

    public enum CircleColors
    {
        Red = 0,
        Green,
        Blue,
        Yellow,
        Magenta,
        Cyan,
    }
}
