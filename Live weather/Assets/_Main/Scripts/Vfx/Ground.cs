using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Ground : MonoBehaviour
{
    // Fields

    [SerializeField] private Image _ground;
    [SerializeField] private Image _snow;
    [SerializeField] private Color _liteGroundColor;
    [SerializeField] private Color _darkGroundColor;
    [SerializeField] private Color _liteSnowColor;
    [SerializeField] private Color _darkSnowColor;

    bool _snowEnabled = false;


    // Public methods

    public void SetGroundDarkness(float hour)
    {
        if (_snowEnabled)
            _snow.DOColor(Color.Lerp(_liteSnowColor, _darkSnowColor, hour), 2);
        else
            _ground.DOColor(Color.Lerp(_liteGroundColor, _darkGroundColor, hour), 2);
    }

    public void EnableSnow()
    {
        if (_snow.fillAmount == 0)
            _snow.DOFillAmount(1, 2).OnComplete(() => _snowEnabled = true);
    }

    public void DisableSnow()
    {
        if (_snow.fillAmount == 1)
            _snow.DOFillAmount(0, 0.5f).OnComplete(() => _snowEnabled = false);
    }
}
