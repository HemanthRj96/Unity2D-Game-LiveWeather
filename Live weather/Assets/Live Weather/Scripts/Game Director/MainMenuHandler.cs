using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuHandler : MonoBehaviour
{
    // Fields

    [SerializeField] private Button _seeButton;
    [SerializeField] private Button _quitButton;


    // Lifecycle methods

    private void Awake()
    {
        _seeButton.onClick.AddListener(() => SceneManager.LoadScene("MainScene", LoadSceneMode.Single));
        _quitButton.onClick.AddListener(() => Application.Quit());
    }
}
