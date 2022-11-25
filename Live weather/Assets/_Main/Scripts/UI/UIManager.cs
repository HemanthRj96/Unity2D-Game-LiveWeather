using System.Collections;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    // Fields

    private IEnumerator updateUI()
    {
        //float timer = 0;
        //if (_hasLoaded)
        //{
        //    var description = (WebDataFetcher.WeatherDescription)_webData.weatherData.current.weather_code;
        //    float hour = _webData.timeData.hour;
        //    float minute = _webData.timeData.minute;
        //    string ap;

        //    if (hour > 12)
        //    {
        //        hour -= 12;
        //        ap = "PM";
        //    }
        //    else ap = "AM";

        //    _informationText.text = $"Weather Description : {description}\n" +
        //                            $"Time : {hour}:{minute} {ap}\n" +
        //                            $"Region : {_webData.locationInfo.regionName}\n" +
        //                            $"City : {_webData.locationInfo.city}\n" +
        //                            $"Lat/Lon : ({_webData.locationInfo.lat},{_webData.locationInfo.lon})";

        //    _statusText.text = "Load Complete!";
        //    yield return new WaitForSeconds(0.5f);
        //    while (_group.alpha > 0)
        //    {
        //        yield return new WaitForSeconds(0.01f);
        //        _group.alpha = Mathf.Lerp(1, 0, timer / 2);
        //        timer += 0.1f;
        //    }
        //}
        //else
        //{
        //    _statusText.text = "Load failed! Check if active internet connection is available.";
        //    yield break;
        //}
        yield return null;
    }
}
