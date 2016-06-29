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
        }

        private void OnNewLevelSet(Notification notification)
        {
            var args = notification.GetArgs<NotificationArgsNewLevel>();
            AssignNewLevelDifficulty(args.level);
            
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
            float radius = Model.GetRadius();
            int score = Model.GetScore(radius);
            circleView.Init(radius, score);
            

        }
    }
}

