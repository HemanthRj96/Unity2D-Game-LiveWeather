using UnityEngine;


public class Vfx : MonoBehaviour
{
    // Fields

    [Header("Clouds")]
    [Space(5)]
    public Clouds fewClouds;
    public Clouds scatteredClouds;
    public Clouds brokenClouds;
    public Clouds overcastClouds;
    [Space(3)]
    [Header("Particle systems")]
    [Space(5)]
    [SerializeField] private ParticleSystem _rain;
    [SerializeField] private ParticleSystem _snow;
    [SerializeField] private ParticleSystem _drizzle;
    [SerializeField] private ParticleSystem _thunderstorm;
    [SerializeField] private ParticleSystem _mist;
    [SerializeField] private ParticleSystem _stars;


    // Public methods

    public void ToggleRain()
    {
        _rain.Play();
        ToggleClouds("broken clouds");
    }

    public void ToggleSnow()
    {
        _snow.Play();
    }

    public void ToggleDrizzle()
    {
        _drizzle.Play();
        ToggleClouds("scattered clouds");
    }

    public void ToggleThunderstorm()
    {
        _thunderstorm.Play();
        ToggleClouds("overcast clouds");
    }

    public void ToggleMist()
    {
        _mist.Play();
    }

    public void ToggleStars()
    {
        _stars.Play();
    }

    public void ToggleClouds(string cloudType)
    {
        switch (cloudType)
        {
            case "few clouds":
                fewClouds.gameObject.SetActive(true);
                fewClouds.EnableClouds();
                break;
            case "scattered clouds":
                scatteredClouds.gameObject.SetActive(true);
                scatteredClouds.EnableClouds();
                break;
            case "broken clouds":
                brokenClouds.gameObject.SetActive(true);
                brokenClouds.EnableClouds();
                break;
            case "overcast clouds":
                overcastClouds.gameObject.SetActive(true);
                overcastClouds.EnableClouds();
                break;
        }
    }

    public void DeactivateAll()
    {
        _rain.Stop();
        _snow.Stop();
        _drizzle.Stop();
        _thunderstorm.Stop();
        _mist.Stop();

        fewClouds.DisableClouds();
        scatteredClouds.DisableClouds();
        brokenClouds.DisableClouds();
        overcastClouds.DisableClouds();

        fewClouds.gameObject.SetActive(false);
        scatteredClouds.gameObject.SetActive(false);
        brokenClouds.gameObject.SetActive(false);
        overcastClouds.gameObject.SetActive(false);
    }
}
