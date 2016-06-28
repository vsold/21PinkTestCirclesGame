using UnityEngine;

namespace CirclesGame
{
    public class UserInputHandler : MonoBehaviour
    {
        public bool Block { get; set; }
        private Camera cam;
        private GameControl controller;

        private const string clickTag = "Circle";

        public void Init(GameControl _controller)
        {
            controller = _controller;
        }

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

            var point = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider = Physics2D.OverlapPoint(point);

            if (collider == null)
            {
                return;
            }

            string tag = collider.gameObject.tag;

            if (string.IsNullOrEmpty(tag) || tag != "clickTag")
                return;
            NotificationCenter.Instance.PostNotification(this, NotificationName.ON_CIRCLE_CLICK);
            //controller.OnUserInput(point, collider);
        }
    }
}
