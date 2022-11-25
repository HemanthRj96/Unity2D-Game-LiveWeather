using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Sun : MonoBehaviour
{
    // Fields

    [SerializeField] private SpriteRenderer _circle;
    [SerializeField] private SpriteRenderer _rays;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private List<SunColors> _colors = new List<SunColors>();
    [SerializeField] private Vector2 _offset;

    bool _isSunEnabled = false;
    bool _canTween = true;
    float _hour = -1;
    Vector2 _originalPosition;


    // Public methods

    public void SetSunTime(float hour)
    {
        _hour = hour;
    }

    public void EnableSun()
    {
        transform.DOMove(_originalPosition + _offset, 1).SetEase(Ease.OutCubic).OnComplete(() => _isSunEnabled = true);
    }

    public void DisableSun()
    {
        transform.DOMove(_originalPosition, 0.5f).SetEase(Ease.OutCubic).OnComplete(() => _isSunEnabled = false);
    }


    // Private methods

    private SunColors getSunColor()
    {
        foreach (var c in _colors)
            if (_hour > c.hourRange.x && _hour <= c.hourRange.y)
                return c;
        return _colors[0];
    }


    // Lifecycle methods

    private void Awake()
    {
        _circle.color = _colors[0].brightColor;
        _rays.color = _colors[0].dimColor;
        _originalPosition = transform.position;
    }

    private void Update()
    {
        if (_isSunEnabled == false)
            return;

        float oldRot = _rays.transform.eulerAngles.z;
        _rays.transform.rotation = Quaternion.Euler(0, 0, oldRot + (Mathf.Rad2Deg * _rotationSpeed * Time.deltaTime));

        if (_canTween)
        {
            _canTween = false;
            Sequence s = DOTween.Sequence();

            s.Append(_rays.DOColor(getSunColor().brightColor, getSunColor().colorBlendTime));
            s.Join(_circle.DOColor(getSunColor().dimColor, getSunColor().colorBlendTime));
            s.Join(_rays.transform.DOScale(2.7f, getSunColor().colorBlendTime));
            
            s.Append(_rays.DOColor(getSunColor().dimColor, getSunColor().colorBlendTime));
            s.Join(_circle.DOColor(getSunColor().brightColor, getSunColor().colorBlendTime));
            s.Join(_rays.transform.DOScale(3f, getSunColor().colorBlendTime));

            s.OnComplete(() => _canTween = true);
        }
    }


    // Nested types

    [System.Serializable]
    private struct SunColors
    {
        public Vector2 hourRange;
        public Color dimColor;
        public Color brightColor;
        public float colorBlendTime;
    }
}
