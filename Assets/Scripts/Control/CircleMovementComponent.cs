using UnityEngine;
using System.Collections;

namespace CirclesGame
{
    public class CircleMovementComponent : MonoBehaviour
    {
        public bool IsMoving { private set; get; }
        public float Speed { set; get; }
        private Vector3 target;
        private Transform cashedTransform;
        private Bounds bounds;
        private Coroutine moveCoroutine;
        private CircleCollider2D circleCollider2D;

        private void Awake()
        {
            cashedTransform = transform;
        }

        private void Start()
        {
            
        }

        public void StartMove(float speed)
        {
            circleCollider2D = gameObject.GetComponent<CircleCollider2D>();
            if (circleCollider2D != null)
            {
                bounds = circleCollider2D.bounds;
            }

            Speed = speed;
            Vector2 screenToCamsize = new Vector2(Screen.width, Screen.height);
            screenToCamsize = Camera.main.ScreenToWorldPoint(screenToCamsize);

            Debug.Log(speed);

            float x = Random.Range(-screenToCamsize.x + bounds.extents.x, screenToCamsize.x - bounds.extents.x);
            float y = screenToCamsize.y - bounds.extents.y;
            float z = cashedTransform.localPosition.z;
            cashedTransform.localPosition = new Vector3(x, y, z);

            target = cashedTransform.position + new Vector3(0f, -screenToCamsize.y * 2 + bounds.size.y, 0f);

            if (moveCoroutine == null)
                moveCoroutine = StartCoroutine(Move());
            IsMoving = true;
        }

        public void StopMove()
        {
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
            moveCoroutine = null;
            IsMoving = false;
            StopMove();
        }
    }

}

