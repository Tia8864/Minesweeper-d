using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIController : MonoBehaviour
{
    [Header("LoadScene")]
    public GameObject _LoadScene;
    public TextMeshProUGUI txtLoad;
    public Slider Slider;

    [Header("Slider")] 
    public TextMeshProUGUI txtVol;
    public Slider SliderVol;
    public TextMeshProUGUI txtRow;
    public Slider SliderRow;
    public TextMeshProUGUI txtCol;
    public Slider SliderCol;
    public TextMeshProUGUI txtMine;
    public Slider SliderMine;


    [SerializeField] private UnitsData LoadData;

    private void Update()
    {
        if(txtVol != null)
            txtVol.SetText(Mathf.RoundToInt(SliderVol.value) + "");
        if (txtRow != null)
        {
            txtRow.SetText(Mathf.RoundToInt(SliderRow.value) + "");
            txtCol.SetText(Mathf.RoundToInt(SliderCol.value) + "");
            txtMine.SetText(Mathf.RoundToInt(SliderMine.value) + "");
        }
    }

    public void _btnQuit()
    {
        Application.Quit();
    }

    public void _loadSreen(int indexScene)
    {
        StartCoroutine(LoadScrene(indexScene));
    }

    IEnumerator LoadScrene(int indexScene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(indexScene);
        float progress = 0;
        _LoadScene.SetActive(true);
        while (!operation.isDone)
        {
            progress = Mathf.Clamp01(operation.progress / 0.9f);
            Slider.value = progress;
            txtLoad.SetText(progress + "%");

            yield return null;
        }
    }

    public void _btnEazy()
    {
        LoadData.Row = 9;
        LoadData.Col = 9;
        LoadData.Mine = 10; 
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        _loadSreen(1);
    }

    public void _btnMedium()
    {

        LoadData.Row = 16;
        LoadData.Col = 16;
        LoadData.Mine = 40;
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        _loadSreen(1);
    }

    public void _btnHard()
    {
        LoadData.Row = 30;
        LoadData.Col = 30;
        LoadData.Mine = 200;
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        _loadSreen(1);
    }

    public void _btnCustom()
    {
        LoadData.Row = Mathf.RoundToInt(SliderRow.value);
        LoadData.Col = Mathf.RoundToInt(SliderRow.value);
        LoadData.Mine = Mathf.RoundToInt(SliderRow.value);
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        _loadSreen(1);
    }


    
}   
