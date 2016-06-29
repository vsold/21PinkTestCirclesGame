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
}

