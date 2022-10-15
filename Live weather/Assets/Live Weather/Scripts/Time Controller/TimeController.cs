using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeController : MonoBehaviour
{
    // Fields

    [SerializeField] private WebDataFetcher _webData;

    [Header("Sky settings")]
    [Space(10)]
    [SerializeField] private List<ColorData> _colorData = new List<ColorData>();
    [SerializeField] private Image _temperatureImage;
    [SerializeField] private Color _defaultColor;

    [Header("Sun and moon settings")]
    [Space(10)]
    [SerializeField] private Transform _sunTransform;
    [SerializeField] Color _sunStartingColor;
    [SerializeField] Color _sunPeakColor;
    [SerializeField] private Transform _moonTransform;

    private bool _hasLoaded = false;


    // Lifecycle methods

    private void Awake()
    {
        _temperatureImage.color = _defaultColor;
    }

    private IEnumerator Start()
    {
        if (_webData == null)
            yield break;
        else
        {
            yield return new WaitUntil(checkState);
            updateTime();
        }
    }


    // private methods

    private bool checkState()
    {
        switch (_webData.GetCurrentState())
        {
            case WebDataFetcher.DataState.loading:
                return false;
            case WebDataFetcher.DataState.hasLoaded:
                _hasLoaded = true;
                return true;
            case WebDataFetcher.DataState.networkError:
                _hasLoaded = false;
                return true;
            default:
                _hasLoaded = false;
                return true;
        }
    }

    private void updateTime()
    {
        if (_hasLoaded)
        {
            float hour = _webData.timeData.hour;
            float minute = _webData.timeData.minute;

            // Sky update
            for (int i = 0; i < _colorData.Count; i++)
            {
                if (_colorData[i].time == hour)
                {
                    Color a = _colorData[i].color;
                    Color b = _colorData[i].color;
                    a.a = 100;
                    b.a = 100;
                    _temperatureImage.color = Color.Lerp(a, b, minute / 60);
                    break;
                }
            }

            // Sun and moon update
            if (hour >= 6 && hour < 18)
            {
                var sp = _sunTransform.GetComponent<SpriteRenderer>();
                _sunTransform.gameObject.SetActive(true);

                if (hour >= 6 && hour < 12)
                {
                    float h = hour - 6;
                    h += minute / 60;
                    sp.color = Color.Lerp(_sunStartingColor, _sunPeakColor, h / 6);
                }
                else
                {
                    float h = hour - 12;
                    h += minute / 60;
                    sp.color = Color.Lerp(_sunPeakColor, _sunStartingColor, h / 6);
                }
            }
            else
            {
                _moonTransform.gameObject.SetActive(true);
            }
        }
    }


    // Nested types

    [System.Serializable]
    public class ColorData
    {
        public Color color;
        public float time;
    }
}