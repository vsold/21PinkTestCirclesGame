using UnityEngine;
using System.Collections;

public class CircleView : MonoBehaviour
{
    public float Radius {
        set
        {
            radius = value;
            CashedTransform.localScale = new Vector3(radius, radius, radius);
        }
        get { return radius; }
    }

    public int Score { set; get; }

    public Color Color
    {
        set
        {
            color = value;
            if (renderer != null)
            {
                renderer.color = Color;
            }
        } 
        get { return color; }
    }


    [SerializeField] 
    private new SpriteRenderer renderer;
    private Color color;
    private float radius;
    private Transform cashedTransform;
    private Transform CashedTransform
    {
        get
        {
            return cashedTransform ?? (cashedTransform = gameObject.transform);
        }
    }

    public void Init(float radius, int score)
    {
        Radius = radius;
        Score = score;
        Color = new Color(Random.value, Random.value, Random.value);

        SetRandomPosition();
    }

    private void SetRandomPosition()
    {
        float x = Random.Range(-300f, 300f);
        float y = Random.Range(-300f, 300f);
        float z = CashedTransform.localPosition.z;

        CashedTransform.localPosition = new Vector3(x, y, z);
    }
}
