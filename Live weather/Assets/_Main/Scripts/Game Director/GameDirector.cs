using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;


public class GameDirector : MonoBehaviour
{
    public int code = 0;
    public string des;

    [Header("Controlled objects")]
    [Space(5)]
    public WebData webData;
    public Ground ground;
    public Sun sun;
    public Moon moon;
    public Sky sky;


    [Header("UI")]
    public Button loadButton;
    public TMP_Text loadText;
    public Image webDataErrorPage;
    public Button retryButton;
    public RectTransform infoRoot;
    public TMP_Text locationInfo;
    public TMP_Text weatherInfo;
    public TMP_Text timeInfo;


    [Header("Vfx System")]
    [Space(5)]
    public Vfx vfx;


    private void Awake()
    {
        var color = loadText.color;
        webData.transform.localScale = Vector3.zero;
        webData.gameObject.SetActive(true);
        infoRoot.localScale = Vector3.zero;
        infoRoot.gameObject.SetActive(true);

        retryButton.onClick.AddListener(() =>
        {
            webData.transform.DOScale(0, 0.2f).SetEase(Ease.OutCubic);
            webData.LoadData();
            StartCoroutine(dataUpdateRoutine());
        });

        loadButton.onClick.AddListener(() =>
        {
            loadButton.interactable = false;
            loadText.color = color;
            loadText.text = "Loading...";

            Sequence s = DOTween.Sequence();
            s.Append(loadText.transform.DOScale(0.9f, 0.25f).SetEase(Ease.InOutCubic));
            s.Append(loadText.transform.DOScale(1, 0.25f).SetEase(Ease.InOutCubic));
            s.AppendInterval(0.25f);
            s.SetLoops(-1, LoopType.Yoyo);
            webData.LoadData();
            StartCoroutine(dataUpdateRoutine());
        });
    }

    private IEnumerator dataUpdateRoutine()
    {
        yield return new WaitUntil(() => webData.completed);

        if (webData.success == false)
        {
            webDataErrorPage.transform.DOScale(1, 0.2f).SetEase(Ease.OutBounce);
            loadButton.transform.DOScale(0, 0.2f).SetEase(Ease.OutCubic);
            yield break;
        }

        loadButton.transform.DOScale(0, 0.2f).SetEase(Ease.OutCubic).OnComplete(() => loadButton.gameObject.SetActive(false));

        // buffer time
        yield return new WaitForSeconds(1);

        infoRoot.DOScale(1, 0.25f).SetEase(Ease.OutCubic);

        locationInfo.text = $"{webData.coordData.regionName}, {webData.coordData.city}, [{webData.coordData.lat}, {webData.coordData.lon}]";
        weatherInfo.text = $"{webData.weatherData.weather[0].description}";
        timeInfo.text = $"{webData.timeData.dateTime}";

        DateTime time = webData.timeData.dateTime;

        // set sky
        sky.SetskyColor(time.Hour);

        // set sun or moon
        if (time.Hour < 18 && time.Hour > 5)
        {
            sun.EnableSun();
            sun.SetSunTime(time.Hour);
        }
        else
        {
            moon.EnableMoon();
            moon.SetMoonTime(time.Hour);
        }

        // activate particle systems
        if (webData.weatherData.weather.Length > 0)
        {
            int code = webData.weatherData.weather[0].id / 100;
            string description = webData.weatherData.weather[0].description;

            vfx.DeactivateAll();
            switch (code)
            {
                case 2:
                    vfx.ToggleThunderstorm();
                    break;
                case 3:
                    vfx.ToggleDrizzle();
                    break;
                case 5:
                    vfx.ToggleRain();
                    break;
                case 6:
                    vfx.ToggleSnow();
                    break;
                case 7:
                    vfx.ToggleMist();
                    break;
                case 8:
                    if (description != "clear sky")
                        vfx.ToggleClouds(description);
                    break;
                default:
                    break;
            }
        }
        else
        {
            // weather error
        }
    }
}
