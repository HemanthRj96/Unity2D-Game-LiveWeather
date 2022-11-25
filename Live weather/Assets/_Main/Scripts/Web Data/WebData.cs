using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class WebData : MonoBehaviour
{
    // Fields

    public LocationInfo coordData;
    public Weather weatherData;
    public TimeDataHelper timeDataHelper;
    public TimeData timeData;
    public string ip;


    // Properties

    public WebData webData { get; private set; }
    public bool completed { get; private set; }
    public bool success { get; private set; }


    // Public methods

    public void LoadData()
    {
        if (completed)
            return;
        StartCoroutine(makeWebRequest());
    }


    // Private methods

    private IEnumerator makeWebRequest()
    {
        success = false;

        UnityWebRequest ipRequest = new UnityWebRequest("https://api.ipify.org/")
        { downloadHandler = new DownloadHandlerBuffer() };
        UnityWebRequest coordRequest = new UnityWebRequest("http://ip-api.com/json/" + ip)
        { downloadHandler = new DownloadHandlerBuffer() };
        UnityWebRequest weatherRequest = new UnityWebRequest($"https://api.openweathermap.org/data/2.5/weather?lat={coordData.lat}&lon={coordData.lon}&appid=10c7ea99bc90ecb33803073aa7a01867")
        { downloadHandler = new DownloadHandlerBuffer() };
        UnityWebRequest timeRequest = new UnityWebRequest($"http://worldtimeapi.org/api/ip")
        { downloadHandler = new DownloadHandlerBuffer() };

        yield return ipRequest.SendWebRequest();
        ip = ipRequest.downloadHandler.text;
        if (ipRequest.responseCode / 100 != 2 || ip == null)
        {
            ipFetchError();
            completed = true;
            yield break;
        }

        yield return coordRequest.SendWebRequest();
        coordData = JsonUtility.FromJson<LocationInfo>(coordRequest.downloadHandler.text);
        if (coordRequest.responseCode / 100 != 2 || coordData == null)
        {
            coordFetchError();
            completed = true;
            yield break;
        }

        yield return weatherRequest.SendWebRequest();
        weatherData = JsonUtility.FromJson<Weather>(weatherRequest.downloadHandler.text);
        if (weatherRequest.responseCode / 100 != 2 || weatherData == null)
        {
            weatherFetchError();
            completed = true;
            yield break;
        }

        yield return timeRequest.SendWebRequest();
        timeDataHelper = JsonUtility.FromJson<TimeDataHelper>(timeRequest.downloadHandler.text);
        timeData = new TimeData(timeDataHelper.unixtime);
        if (timeRequest.responseCode / 100 != 2 || timeData == null)
        {
            timeFetchError();
            completed = true;
            yield break;
        }

        success = true;
        completed = true;
    }

    private void ipFetchError()
    {
    }

    private void coordFetchError()
    {
    }

    private void weatherFetchError()
    {
    }

    private void timeFetchError()
    {
    }


    // Lifecycle methods

    private void Awake()
    {
        if (webData == null)
            webData = this;
        else
            Destroy(gameObject);
    }


    // Nested types

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
    public class Weather
    {
        public WeatherData[] weather;
    }

    [System.Serializable]
    public class WeatherData
    {
        public int id;
        public string main;
        public string description;
        public string icon;
    }

    [System.Serializable]
    public class TimeDataHelper
    {
        public string abbreviation;
        public string client_ip;
        public string dateTime;
        public int day_of_week;
        public int day_of_year;
        public bool dst;
        public string dst_from;
        public int dst_offset;
        public string dst_until;
        public int raw_offset;
        public string timezone;
        public int unixtime;
        public string utc_datetime;
        public string utc_offset;
        public int week_number;
    }

    [System.Serializable]
    public class TimeData
    {
        public TimeData(int unixTime)
        {
            dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTime).ToLocalTime();
        }

        public DateTime dateTime;
    }
}