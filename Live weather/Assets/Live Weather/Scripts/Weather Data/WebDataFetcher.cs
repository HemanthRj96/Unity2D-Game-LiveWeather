using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WebDataFetcher : MonoBehaviour
{
    // Fields

    public LocationInfo locationInfo;
    public WeatherData weatherData;
    public TimeData timeData;

    private string _ip;
    public DataState _dataState = DataState.loading;


    // Public methods

    public DataState GetCurrentState() => _dataState;


    // Lifecycle methods

    private void Start() => beginFetch();


    // Private methods

    private void beginFetch() => StartCoroutine(getIP());

    private IEnumerator getIP()
    {
        UnityWebRequest www = new UnityWebRequest("https://api.ipify.org/")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("Failed to connect");
            _dataState = DataState.networkError;
            yield break;
        }

        _ip = www.downloadHandler.text;

        if (_ip == null)
        {
            _dataState = DataState.networkError;
            yield break;
        }
        else
            StartCoroutine(getCoordinates());
    }

    private IEnumerator getCoordinates()
    {
        UnityWebRequest www = new UnityWebRequest("http://ip-api.com/json/" + _ip)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("Failed to connect");
            _dataState = DataState.networkError;
            yield break;
        }

        locationInfo = JsonUtility.FromJson<LocationInfo>(www.downloadHandler.text);

        if (locationInfo.status == "failed" || locationInfo.status == null)
        {
            _dataState = DataState.networkError;
            yield break;
        }
        else
            StartCoroutine(getWeatherData());
    }

    private IEnumerator getWeatherData()
    {
        UnityWebRequest www = new UnityWebRequest("http://api.weatherstack.com/current?" +
                                                  "access_key=1758b80fa6ecae91c0bb97ca20beb5c8&" +
                                                  $"query={locationInfo.lat},{locationInfo.lon}")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("Failed to connect");
            _dataState = DataState.networkError;
            yield break;
        }

        weatherData = null;
        weatherData = JsonUtility.FromJson<WeatherData>(www.downloadHandler.text);

        if (weatherData.current.observation_time == null)
        {
            _dataState = DataState.networkError;
            yield break;
        }
        else
            StartCoroutine(getTimeData());
    }

    private IEnumerator getTimeData()
    {
        UnityWebRequest www = new UnityWebRequest($"https://www.timeapi.io/api/Time/current/coordinate?latitude={locationInfo.lat}&longitude={locationInfo.lon}")
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogWarning("Failed to connect");
            _dataState = DataState.networkError;
            yield break;
        }

        timeData = JsonUtility.FromJson<TimeData>(www.downloadHandler.text);

        if (timeData.year == 0)
        {
            _dataState = DataState.networkError;
            yield break;
        }
        else
            _dataState = DataState.hasLoaded;
    }


    // Nested types

    public enum DataState
    {
        loading,
        hasLoaded,
        networkError
    }

    public enum WeatherDescription
    {
        defaultNull,
        Sunny = 113,
        Partlycloudy = 116,
        Cloudy = 119,
        Overcast = 122,
        Mist = 143,
        PatchyRainPossible = 176,
        PatchySnowPossible = 179,
        PatchySleetPossible = 182,
        ThunderyOutbreaksPossible = 200,
        BlowingSnow = 227,
        Blizzard = 230,
        Fog = 248,
        FreezingFog = 260,
        PatchyLightDrizzle = 263,
        LightDrizzle = 266,
        FreezingDrizzle = 281,
        HeavyFreezingDrizzle = 284,
        PatchyLightRain = 293,
        LightRain = 296,
        ModerateRainAtTimes = 299,
        ModerateRain = 302,
        HeavyRainAtTimes = 305,
        HeavyRain = 308,
        LightFreezingRain = 311
    }

    [System.Serializable]
    public class TimeData
    {
        public float year;
        public float month;
        public float day;
        public float hour;
        public float minute;
        public float seconds;
        public float milliSeconds;
        public string dateTime;
        public string date;
        public string time;
        public string timeZone;
        public string dayOfWeek;
        public bool dstActive;
    }

    [System.Serializable]
    public class LocationInfo
    {
        public string query;
        public string status;
        public string country;
        public string countryCode;
        public string region;
        public string regionName;
        public string city;
        public string zip;
        public float lat;
        public float lon;
        public string timezone;
        public string isp;
        public string org;
        public string @as;
    }

    [System.Serializable]
    public class WeatherData
    {
        public WeatherInfo current = null;
    }

    [System.Serializable]
    public class WeatherInfo
    {
        public string observation_time;
        public float temperature;
        public int weather_code;
        public List<string> weather_icons = new List<string>();
        public List<string> weather_descriptions = new List<string>();
        public float wind_speed;
        public float wind_degree;
        public string wind_dir;
        public float pressure;
        public float precip;
        public float humidity;
        public float cloudcover;
        public float feelslike;
        public float uv_index;
        public float visibility;
    }
}