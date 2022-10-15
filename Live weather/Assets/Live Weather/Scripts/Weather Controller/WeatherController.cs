using System.Collections;
using UnityEngine;


public class WeatherController : MonoBehaviour
{
    // Fields

    [SerializeField] private WebDataFetcher _webData;

    [Header("Clouds prefabs")]
    [Space(10)]
    [SerializeField] private GameObject _clear;
    [SerializeField] private GameObject _cloudy;
    [SerializeField] private GameObject _partlyCloudy;
    [SerializeField] private GameObject _overcast;

    [Header("Ground Prefabs")]
    [Space(10)]
    [SerializeField] private GameObject _normalGround;
    [SerializeField] private GameObject _partlySnowyGround;
    [SerializeField] private GameObject _snowyGround;
    [SerializeField] private GameObject _heavySnowyGround;
    [SerializeField] private GameObject _partlyCloudyGround;
    [SerializeField] private GameObject _cloudyGround;
    [SerializeField] private GameObject _nightGround;

    [Header("Rain Prefabs")]
    [Space(10)]
    [SerializeField] private GameObject _liteRain;
    [SerializeField] private GameObject _mediumRain;
    [SerializeField] private GameObject _heavyRain;

    [Header("Snow Prefabs")]
    [SerializeField] private GameObject _liteSnow;
    [SerializeField] private GameObject _blowingSnow;

    [Header("Mist Prefabs")]
    [Space(10)]
    [SerializeField] private GameObject _mistScreen;
    [SerializeField] private GameObject _fogScreen;

    private bool _hasLoaded = false;


    // Lifecycle methods

    private void Awake()
    {
        _clear.SetActive(false);
    }

    private IEnumerator Start()
    {
        if (_webData == null)
            yield break;
        else
        {
            yield return new WaitUntil(checkState);
            updateWeather();
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

    private void updateWeather()
    {
        if (_hasLoaded)
        {
            WebDataFetcher.WeatherDescription wd = (WebDataFetcher.WeatherDescription)_webData.weatherData.current.weather_code;

            switch (wd)
            {
                case WebDataFetcher.WeatherDescription.defaultNull:
                    break;
                case WebDataFetcher.WeatherDescription.Sunny:
                    {
                        _clear.SetActive(true);
                        _normalGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Partlycloudy:
                    {
                        _partlyCloudy.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Cloudy:
                    {
                        _cloudy.SetActive(true);
                        _cloudyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Overcast:
                    {
                        _overcast.SetActive(true);
                        _nightGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Mist:
                    {
                        _clear.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        _mistScreen.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.PatchyRainPossible:
                    {
                        _partlyCloudy.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        _liteRain.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.PatchySnowPossible:
                    {
                        _clear.SetActive(true);
                        _partlySnowyGround.SetActive(true);
                        _liteSnow.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.PatchySleetPossible:
                    {
                        _clear.SetActive(true);
                        _partlySnowyGround.SetActive(true);
                        _liteSnow.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.ThunderyOutbreaksPossible:
                    {
                        _overcast.SetActive(true);
                        _cloudyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.BlowingSnow:
                    {
                        _clear.SetActive(true);
                        _snowyGround.SetActive(true);
                        _blowingSnow.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Blizzard:
                    {
                        _partlyCloudy.SetActive(true);
                        _fogScreen.SetActive(true);
                        _blowingSnow.SetActive(true);
                        _heavySnowyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.Fog:
                    {
                        _clear.SetActive(true);
                        _fogScreen.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.FreezingFog:
                    {
                        _clear.SetActive(true);
                        _fogScreen.SetActive(true);
                        _partlySnowyGround.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.PatchyLightDrizzle:
                case WebDataFetcher.WeatherDescription.LightDrizzle:
                    {
                        _partlyCloudyGround.SetActive(true);
                        _partlyCloudy.SetActive(true);
                        _liteRain.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.FreezingDrizzle:
                case WebDataFetcher.WeatherDescription.LightFreezingRain:
                case WebDataFetcher.WeatherDescription.HeavyFreezingDrizzle:
                    {
                        _partlyCloudy.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        _mediumRain.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.PatchyLightRain:
                case WebDataFetcher.WeatherDescription.LightRain:
                    {
                        _partlyCloudy.SetActive(true);
                        _partlyCloudyGround.SetActive(true);
                        _liteRain.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.ModerateRainAtTimes:
                case WebDataFetcher.WeatherDescription.ModerateRain:
                    {
                        _partlyCloudyGround.SetActive(true);
                        _cloudy.SetActive(true);
                        _mediumRain.SetActive(true);
                        break;
                    }
                case WebDataFetcher.WeatherDescription.HeavyRainAtTimes:
                case WebDataFetcher.WeatherDescription.HeavyRain:
                    {
                        _cloudyGround.SetActive(true);
                        _overcast.SetActive(true);
                        _heavyRain.SetActive(true);
                        break;
                    }
            }
        }
    }
}
