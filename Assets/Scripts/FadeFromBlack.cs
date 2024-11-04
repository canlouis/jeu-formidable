using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeFromBlack : MonoBehaviour
{
    private Image _img;
    private float _alpha =1f;

    [SerializeField]
    [Range(0.1f, 5f)]
    private float _fadeSpeed;

    void Start()
    {
        _img = GetComponent<Image>();
        ChangeAlpha(_alpha);
    }

    void Update()
    {
        _alpha -= Time.deltaTime * _fadeSpeed;

        if (_alpha <= 0f)
        {
            ChangeAlpha(0f);
        }
        else
        {
            ChangeAlpha(_alpha);
        }

    }

    private void ChangeAlpha(float value)
    {
        _img.color = new Color(_img.color.r, _img.color.g, _img.color.b, value);
    }
}
