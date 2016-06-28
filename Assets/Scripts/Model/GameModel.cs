using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class GameModel
    {
        public readonly Vector2 radiusBounds = new Vector2(10f, 60f);
        public const float baseSpeed = 100f;
        public const int startCount = 5;
        public const float startSpawnTime = 1f;

        public int Score { set; get; }
    }
}
