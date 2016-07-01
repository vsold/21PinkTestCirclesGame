using UnityEngine;

namespace CirclesGame
{
    public abstract class NotificationArgs
    {
    }

    public class NotificationArgsEmpty : NotificationArgs
    {
        public NotificationArgsEmpty()
        {
        }
    }

    public class NotificationArgsNewLevel : NotificationArgs
    {
        public LevelDifficulty level;

        public NotificationArgsNewLevel(LevelDifficulty level)
        {
            this.level = level;
        }
    }

    public class NotificationArgsScores : NotificationArgs
    {
        public int score;

        public NotificationArgsScores(int score)
        {
            this.score = score;
        }
    }

    public class NotificationArgsOnCircleHitsGround : NotificationArgs
    {
        public GameObject gameObj;

        public NotificationArgsOnCircleHitsGround(GameObject gameObj)
        {
            this.gameObj = gameObj;
        }
    }
}

