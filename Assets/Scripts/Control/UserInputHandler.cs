using UnityEngine;

namespace CirclesGame
{
    public class UserInputHandler : MonoBehaviour
    {
        public bool Block { get; set; }
        private Camera cam;

        private const string CLICK_TAG = "Circle";

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Block)
            {
                return;
            }

            CheckForMouseInput();
        }

        private void CheckForMouseInput()
        {
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(point);

            if (collider == null)
            {
                return;
            }

            string tag = collider.gameObject.tag;

            if (string.IsNullOrEmpty(tag) || tag != CLICK_TAG)
                return;

            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_CIRCLE_CLICK, new NotificationArgsUserInput(collider));
        }
    }
}
