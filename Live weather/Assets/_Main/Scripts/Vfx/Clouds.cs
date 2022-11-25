using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Clouds : MonoBehaviour
{
    // Fields

    [SerializeField] private List<CloudData> _cloudData = new List<CloudData>();
    [SerializeField] private float _offset;
    [SerializeField] private float _cloudInDuration;
    [SerializeField] private float _cloudOutDuration;
    [SerializeField] private Vector2 _leftRightLimit;

    float _y = 0;
    bool _isCloudEnabled = false;


    // Public methods

    public void EnableClouds()
    {
        if (_isCloudEnabled)
            return;

        Sequence s = DOTween.Sequence();

        s.Append(transform.DOMoveY(_y - _offset, _cloudInDuration).SetEase(Ease.OutElastic));
        _isCloudEnabled = true;
    }

    public void DisableClouds()
    {
        if (_isCloudEnabled == false)
            return;
        _isCloudEnabled = false;

        Sequence s = DOTween.Sequence();

        s.Join(transform.DOMoveY(_y, _cloudOutDuration));
    }


    // Private methods

    private void scrollClouds()
    {
        foreach (var c in _cloudData)
        {
            float speed = c.speed;

            if (c.t.position.x < _leftRightLimit.x)
                c.t.position = new Vector2(_leftRightLimit.y, c.t.position.y);
            if (c.t.position.x > _leftRightLimit.y)
                c.t.position = new Vector2(_leftRightLimit.x, c.t.position.y);

            c.t.position += Vector3.right * speed * Time.deltaTime;
        }
    }


    // Lifecycle methods

    private void Awake()
    {
        _y = transform.position.y;
    }


    private void FixedUpdate()
    {
        if (_isCloudEnabled)
            scrollClouds();
    }


    // Nested types 

    [System.Serializable]
    private struct CloudData
    {
        public SpriteRenderer sr;
        public Transform t;
        public float speed;
    }
}

