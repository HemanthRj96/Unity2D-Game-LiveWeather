using System.Collections.Generic;
using UnityEngine;


public class CloudScroller : MonoBehaviour
{
    // Fields

    [Header("Movement settings")]
    [Space(10)]
    [SerializeField] private Vector2 _speedRange;
    [SerializeField] private Vector2 _horizontalRange;

    private Transform _t;
    private List<Clouds> _clouds = new List<Clouds>();


    // Lifecycle methods

    private void Awake()
    {
        _t = transform;

        foreach (Transform child in _t)
        {
            Clouds c = new Clouds()
            {
                t = child,
                speed = Random.Range(_speedRange.x, _speedRange.y) * (Random.Range(0, 2) == 0 ? -1 : 1)
            };
            _clouds.Add(c);
        }
    }


    private void FixedUpdate()
    {
        movementUpdate();
    }


    // Private methods

    private void movementUpdate()
    {
        foreach (var c in _clouds)
        {
            if (c.t.localPosition.x < _horizontalRange.x - 1)
            {
                var oldPos = c.t.localPosition;
                c.t.localPosition = new Vector3(_horizontalRange.y + 1, oldPos.y, oldPos.z);
            }
            else if (c.t.localPosition.x > _horizontalRange.y + 1)
            {
                var oldPos = c.t.localPosition;
                c.t.localPosition = new Vector3(_horizontalRange.x - 1, oldPos.y, oldPos.z);
            }
            c.t.localPosition += Vector3.right * c.speed * Time.deltaTime;
        }
    }


    // Nested types

    public class Clouds
    {
        public Transform t;
        public float speed;
    }
}
