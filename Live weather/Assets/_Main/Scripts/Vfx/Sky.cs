using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Sky : MonoBehaviour
{
    // Fields

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private List<Skycolors> _colors = new List<Skycolors>();
    [SerializeField] private float _colorFadeDuration;


    // Public methods

    public void SetskyColor(float hour)
    {
        _sr.DOColor(getSkyColor(hour), _colorFadeDuration).SetEase(Ease.InOutCubic);
    }


    // Private methods

    private Color getSkyColor(float hour)
    {
        foreach (var c in _colors)
            if (hour >= c.hourRange.x && hour < c.hourRange.y)
                return Color.Lerp(c.colorA, c.colorB, Mathf.InverseLerp(c.hourRange.x, c.hourRange.y, hour));
        return _colors[0].colorA;
    }


    // Nested types

    [System.Serializable]
    private struct Skycolors
    {
        public Vector2 hourRange;
        public Color colorA;
        public Color colorB;
    }
}
