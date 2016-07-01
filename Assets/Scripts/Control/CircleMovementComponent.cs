using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class CircleMovementComponent : MonoBehaviour
    {
        public bool IsMoving { private set; get; }
        public float Speed { set; get; }

        [SerializeField]
        private SphereCollider circleCollider;
        private Vector3 target;
        private Transform cashedTransform;
        private Bounds bounds;
        private Coroutine moveCoroutine;
        private Vector2 screenToCamsize;

        private void Awake()
        {
            cashedTransform = transform;
        }

        private void UpdateScreenSize()
        {
            screenToCamsize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }

        private Vector3 GetStartPoint()
        {
            float x = Random.Range(-screenToCamsize.x + bounds.extents.x, screenToCamsize.x - bounds.extents.x);
            float y = screenToCamsize.y - bounds.extents.y;
            float z = cashedTransform.localPosition.z;
            return new Vector3(x, y, z);
        }

        private Vector3 GetFinalPoint()
        {
            return cashedTransform.position + new Vector3(0f, -screenToCamsize.y*2 + bounds.size.y, 0f);
        }

        public void StartMove(float speed)
        {
            Speed = speed;
            bounds = circleCollider.bounds;
            UpdateScreenSize();

            cashedTransform.localPosition = GetStartPoint();
            target = GetFinalPoint();

            if (moveCoroutine == null)
                moveCoroutine = StartCoroutine(Move());

            IsMoving = true;
        }

        public void StopMove()
        {
            moveCoroutine = null;
            IsMoving = false;
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
        }

        private IEnumerator Move()
        {
            while ((cashedTransform.position - target).magnitude > 0.1f)
            {
                float stepDelta = Speed * Time.deltaTime;
                Vector3 newPos = Vector3.MoveTowards(cashedTransform.position, target, stepDelta);
                transform.position = newPos;
                yield return null;
            }

            cashedTransform.position = target;
            StopMove();
            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_CIRCLE_HIT_GROUND);
        }

        private void OnDisable()
        {
            StopMove();
        }
    }
}

