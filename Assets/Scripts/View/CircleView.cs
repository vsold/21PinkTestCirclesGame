using UnityEngine;
using CirclesGame;

public class CircleView : MonoBehaviour
{
    public CircleSizes Size { get {return size;}}
    public CircleColors Color { get {return color;}}
    public int Score { get {return score;}}
    public MeshRenderer Renderer { get {return renderer;}}

    [SerializeField] private new MeshRenderer renderer;
    private CircleSizes size;
    private CircleColors color;
    private int score;
    private Transform cashedTransform;
    private const int BASE_SIZE = 16;

    void Awake()
    {
        cashedTransform = gameObject.transform;
    }

    public void Init(CircleSizes size, CircleColors color, int score)
    {
        this.size = size;
        this.color = color;
        this.score = score;
        SetActualSize();
    }

    private void SetActualSize()
    {
        int actSize = (int)size * BASE_SIZE;
        cashedTransform.localScale = new Vector3(actSize, actSize, 0);
    }

    private void OnMouseDown()
    {
        NotificationCenter.Instance.PostNotification(this, NotificationName.ON_CIRCLE_CLICK);
    }
}
