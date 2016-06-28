using UnityEngine;
using System.Collections;

public class CircleView : MonoBehaviour
{
    public float Radius { set; get; }
    public Color Color {set; get;}
    public int Score {set; get;}

    [SerializeField]
    private SpriteRenderer renderer;

    public void Init(float radius, int score)
    {
        Radius = radius;
        Score = score;
    }

    private void SetRandomColorValue()
    {
        Color = new Color(Random.value, Random.value, Random.value);
        if (renderer != null)
        {
            renderer.color = Color;
        }
    }

    void Start ()
    {
        SetRandomColorValue();
    }
}
