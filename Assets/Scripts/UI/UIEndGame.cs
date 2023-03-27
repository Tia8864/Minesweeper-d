using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEndGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtEndGame;
    [SerializeField] private UnitsData unitsData;

    private void Awake()
    {
        if (unitsData.WinGame)
        {
            txtEndGame.SetText("Win");
        }
        else
        {
            txtEndGame.SetText("Game Over");
        }
    }


    public void _btnQuit()
    {
        Application.Quit();
    }

    public void _btnPlayAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void _btnBack()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
