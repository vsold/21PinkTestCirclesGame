using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CirclesGame
{
    public class CirclesSpawnerComponent : MonoBehaviour
    {
        public GameModel Model { set; get; }

        [SerializeField] 
        private ObjectPool circlesPool;
        [SerializeField] 
        private TexturesResourceManager resourceManager;
        private LevelDifficulty currenLevelDifficulty;
        private List<GameObject> activeCircles;
        private int pauseCounter = 0;
        private float delayTillNextSpawn = 0f;

        void Awake()
        {
            activeCircles = new List<GameObject>();
            NotificationCenter.Instance.AddObserver(this, OnNewLevelSet, NotificationName.ON_NEW_LEVEl);
            NotificationCenter.Instance.AddObserver(this, OnCircleClick, NotificationName.ON_CIRCLE_CLICK);
            NotificationCenter.Instance.AddObserver(this, OnCircleHitGround, NotificationName.ON_CIRCLE_HIT_GROUND);
        }

        private void OnCircleHitGround(Notification notification)
        {
            DestroyCircle(notification.sender.gameObject);
        }

        private void OnNewLevelSet(Notification notification)
        {
            var args = notification.GetArgs<NotificationArgsNewLevel>();
            AssignNewLevelDifficulty(args.level);
        }

        private void OnCircleClick(Notification notification)
        {
            var circleView = notification.sender.GetComponent<CircleView>();
            if (circleView == null)
                return;
            int score = circleView.Score;
            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_SCORE_INC, new NotificationArgsScores(score));
            DestroyCircle(circleView.gameObject);
        }

        private void DestroyCircle(GameObject circle)
        {
            if (activeCircles.Contains(circle))
                activeCircles.Remove(circle);
            circlesPool.ReturnObject(circle);
        }

        public void Pause()
        {
            pauseCounter += 1;
        }

        public void Resume()
        {
            if (pauseCounter > 0)
                pauseCounter -= 1;
        }

        private void AssignNewLevelDifficulty(LevelDifficulty level)
        {
            currenLevelDifficulty = level;
            ResetDelayTimer();
            Pause();
            resourceManager.GradientColor = (resourceManager.GradientColor == Color.white) ? Color.black : Color.white;
            StartCoroutine(WaitForClearField());
        }

        private IEnumerator WaitForClearField()
        {
            while (activeCircles.Count > 0)
            {
                yield return null;
            }
            resourceManager.GenerateNewSet(OnTexturesReady);
        }

        private void OnTexturesReady()
        {
            Resume();
        }

        private void Update()
        {
            if (pauseCounter > 0 || currenLevelDifficulty == null)
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
            if (newCircle == null)
            {
                Debug.Log("Pool is not ready yet. Loading...");
                return;
            }
            activeCircles.Add(newCircle);
            newCircle.SetActive(true);
            var circleView = newCircle.GetComponent<CircleView>();
            InitCircleView(circleView);
            var circleMove = newCircle.GetComponent<CircleMovementComponent>();
            StartCircleMovement(circleMove, circleView.Size);
        }

        private void InitCircleView(CircleView circleView)
        {
            CircleSizes size = Model.GetSize();
            CircleColors color = Model.GetColor();
            int score = Model.GetScore(size);
            circleView.Init(size, color, score);
            var texture = resourceManager.GetTexture(size, color);
            circleView.Renderer.material.SetTexture("_MainTex", texture);
        }

        private void StartCircleMovement(CircleMovementComponent circleMovement, CircleSizes size)
        {
            float speed = Model.GetSpeed(size);
            circleMovement.StartMove(speed);
        }
    }
}

