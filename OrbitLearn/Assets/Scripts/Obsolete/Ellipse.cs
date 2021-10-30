using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ellipse
{
    // Start is called before the first frame update
    public float xAxis, yAxis;
    public Ellipse (float myxAxis, float myyAxis)
    {
        xAxis = myxAxis;
        yAxis = myyAxis;

    }
    public Vector2 Evaluate(float t)
    {
        float angle = Mathf.Deg2Rad * 360f * t;
        float x = Mathf.Sin(angle) * xAxis;
        float y = Mathf.Cos(angle) * yAxis;
        return new Vector2(x, y);
    }
}
