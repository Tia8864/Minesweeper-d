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

    [Header("UI")]
    public GameObject MenuUI;
    public GameObject PlayUI;
    public GameObject CustomUI;
    public GameObject LoadSceneUI;
    public GameObject OpitionUI;

    [Header("InputData")]
    [SerializeField] private UnitsData LoadData;
    private void Awake()
    {
        if(MenuUI != null) 
            MenuUI.SetActive(true);
        if(PlayUI != null) 
            PlayUI.SetActive(false);
        if(CustomUI != null) 
            CustomUI.SetActive(false);
        if(LoadSceneUI != null) 
            LoadSceneUI.SetActive(false);
        if(OpitionUI != null) 
            OpitionUI.SetActive(false);
        LoadData.IsMute = false;
        SliderCol.value = 9;
        SliderRow.value = 9;
        SliderMine.value = 10;
        SliderVol.value = LoadData.Vol;
    }

    private void Update()
    {
        if(txtVol != null)
        {
            LoadData.Vol = SliderVol.value;
            txtVol.SetText(Mathf.RoundToInt(SliderVol.value) + "");
        }
        if (txtRow != null)
        {
            _OnValidate();
            txtRow.SetText(Mathf.RoundToInt(SliderRow.value) + "");
            txtCol.SetText(Mathf.RoundToInt(SliderCol.value) + "");
            txtMine.SetText(Mathf.RoundToInt(SliderMine.value) + "");
        }
    }

    public void _OnValidate()
    {
        if (Mathf.Abs(SliderCol.value - SliderRow.value) > 5)
        {
            if (SliderCol.value > SliderRow.value)
            {
                SliderCol.value -= 1;
            }


            if (SliderRow.value > SliderCol.value)
            {
                SliderRow.value -= 1;
            }

            if (SliderCol.value < SliderRow.value)
            {
                SliderCol.value += 1;
            }

            if (SliderRow.value < SliderCol.value)
            {
                SliderRow.value += 1;
            }
        }
        if(SliderMine.value < SliderRow.value* SliderCol.value)
        {
            SliderMine.maxValue = SliderRow.value * SliderCol.value;
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
        LoadData.IsTimeDown = true;
        LoadData.Time = 40;
        _loadSreen(1);
    }

    public void _btnMedium()
    {
        LoadData.Row = 16;
        LoadData.Col = 16;
        LoadData.Mine = 40;
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        LoadData.IsTimeDown = true;
        LoadData.Time = 70;
        _loadSreen(1);
    }

    public void _btnHard()
    {
        LoadData.Row = 30;
        LoadData.Col = 30;
        LoadData.Mine = 200;
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        LoadData.IsTimeDown = true;
        LoadData.Time = 100;
        _loadSreen(1);
    }

    public void _btnCustom()
    {
        LoadData.Row = Mathf.RoundToInt(SliderRow.value);
        LoadData.Col = Mathf.RoundToInt(SliderCol.value);
        LoadData.Mine = Mathf.RoundToInt(SliderMine.value);
        LoadData.Vol = Mathf.RoundToInt(SliderVol.value);
        LoadData.IsTimeDown = false;       
        _loadSreen(1);
    }
}   
