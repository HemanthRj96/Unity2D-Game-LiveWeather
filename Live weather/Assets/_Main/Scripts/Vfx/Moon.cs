using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    // Fields

    [SerializeField] private SpriteRenderer _moon;
    [SerializeField] private List<MoonColors> _colors = new List<MoonColors>();
    [SerializeField] private Vector2 _offset;

    bool _isMoonEnabled = false;
    bool _canTween = true;
    float _hour = -1;
    Vector2 _originalPosition;


    // Public methods

    public void SetMoonTime(float hour)
    {
        _hour = hour;
    }

    public void EnableMoon()
    {
        transform.DOMove(_originalPosition + _offset, 1).SetEase(Ease.OutCubic).OnComplete(() => _isMoonEnabled = true);
    }

    public void DisableMoon()
    {
        transform.DOMove(_originalPosition, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => _isMoonEnabled = false);
    }


    // Private methods

    private MoonColors getMoonColor()
    {
        foreach (var c in _colors)
            if (_hour > c.hourRange.x && _hour <= c.hourRange.y)
                return c;
        return _colors[0];
    }


    // Lifecycle methods

    private void Awake()
    {
        _moon.color = _colors[0].brightColor;
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (_isMoonEnabled == false)
            return;

        if (_canTween)
        {
            _canTween = false;
            Sequence s = DOTween.Sequence();

            s.Append(_moon.DOColor(getMoonColor().dimColor, getMoonColor().colorBlendTime));
            s.Append(_moon.DOColor(getMoonColor().brightColor, getMoonColor().colorBlendTime));

            s.OnComplete(() => _canTween = true);
        }
    }


    // Nested types

    [System.Serializable]
    private struct MoonColors
    {
        public Vector2 hourRange;
        public Color dimColor;
        public Color brightColor;
        public float colorBlendTime;
    }
}
