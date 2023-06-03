using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScaller : MonoBehaviour
{
    private int _currentHeight;
    private int _currentWidth;

    [SerializeField] private Camera _camera;

    /// <summary>
    /// Rescales all objects in scene to optimal size, oriented by screen's resolution
    /// </summary>
    void Rescale(Rect rect)
    {
        _currentHeight = Screen.height;
        _currentWidth = Screen.width;

        float targetAspect = 16.0f / 9.0f;

        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }
        else
        {
            float scaleWidth = 1.0f / scaleHeight;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
        }

        _camera.rect = rect;
    }

    void Start()
    {
        _camera = GetComponent<Camera>();
        Rescale(_camera.rect);
    }

    void Update()
    {
        if (_currentHeight != Screen.height || _currentWidth != Screen.width)
        {
            Rescale(_camera.rect);
        }
    }
}
