using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class CirclesSpawnerComponent : MonoBehaviour
    {
        public GameModel Model { set; get; }

        [SerializeField] 
        private ObjectPool circlesPool;
        private LevelDifficulty currenLevelDifficulty;
        private float delayTillNextSpawn = 0f;

        void Awake()
        {
            NotificationCenter.Instance.AddObserver(this, OnNewLevelSet, NotificationName.ON_NEW_LEVEl);
            NotificationCenter.Instance.AddObserver(this, OnCircleClick, NotificationName.ON_CIRCLE_CLICK);
            NotificationCenter.Instance.AddObserver(this, OnCircleHitGround, NotificationName.ON_CIRCLE_HIT_GROUND);
        }

        private void OnCircleHitGround(Notification notification)
        {
            circlesPool.ReturnObject(notification.sender.gameObject);
        }

        private void OnNewLevelSet(Notification notification)
        {
            var args = notification.GetArgs<NotificationArgsNewLevel>();
            AssignNewLevelDifficulty(args.level);
        }

        private void OnCircleClick(Notification notification)
        {
            var args = notification.GetArgs<NotificationArgsUserInput>();
            var circleCollider = args.collider;
            int score = circleCollider.GetComponent<CircleView>().Score;
            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_SCORE_INC, new NotificationArgsScores(score));
            circlesPool.ReturnObject(circleCollider.gameObject);
        }

        private void AssignNewLevelDifficulty(LevelDifficulty level)
        {
            currenLevelDifficulty = level;
            ResetDelayTimer();
        }

        private void Update()
        {
            if (currenLevelDifficulty == null)
                return;

            delayTillNextSpawn -= Time.deltaTime;

            if (delayTillNextSpawn > 0)
                return;

            SpawnCircle();
            ResetDelayTimer();
        }

        private void ResetDelayTimer()
        {
            var spawnInterwal = currenLevelDifficulty.SpawnTimeInterval;
            delayTillNextSpawn = Random.Range(spawnInterwal.x, spawnInterwal.y);
        }

        private void SpawnCircle()
        {
            var newCircle = circlesPool.GetObject();
            newCircle.SetActive(true);
            var circleView = newCircle.GetComponent<CircleView>();
            var circleMove = newCircle.GetComponent<CircleMovementComponent>();
            float radius = Model.GetRadius();
            int score = Model.GetScore(radius);
            float speed = Model.GetSpeed(radius);
            circleView.Init(radius, score);
            circleMove.StartMove(speed);
        }
    }
}

