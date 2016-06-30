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

    public class NotificationArgsUserInput : NotificationArgs
    {
        public Collider2D collider;

        public NotificationArgsUserInput(Collider2D collider)
        {
            this.collider = collider;
        }
    }

    public class NotificationArgsScoresInc : NotificationArgs
    {
        public int score;

        public NotificationArgsScoresInc(int score)
        {
            this.score = score;
        }
    }
}

