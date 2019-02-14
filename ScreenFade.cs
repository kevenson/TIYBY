using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{

    public Material m;
    public float _colorSpeed = 0.01f;
    private Color c;
    public float alphaColor = 0.0f;
    private bool start = false;

    void Start()
    {
        c = m.color;
        c.a = alphaColor;
        m.color = c;
        Debug.Log("Alpha color should be 0: " + c.a);
    }

    void Update()
    {
        if (start)
        {
            if (c.a < 100f)
                c.a = c.a + _colorSpeed;
            m.color = c;
        }
    }
    public void Fade()
    {
        start = true;
    }
}
