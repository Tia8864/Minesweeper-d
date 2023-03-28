using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Game : MonoBehaviour
{
    public int width = 16;
    public int height = 16;
    public int mineCount = 32;
    private int flag = 0;
    private float timeDown;

    [SerializeField] private Board board;
    [SerializeField] private Transform Map;
    private Cell[,] state;

    [SerializeField] private UnitsData LoadData;

    [Header("InGame")]
    [SerializeField] private TextMeshProUGUI txtFlag;
    [SerializeField] private Slider sliderTime;
    [SerializeField] private GameObject sliderTimeUI;
    [SerializeField] private GameObject popUpPasue;
    [SerializeField] private Image imgBtnMute;
    [SerializeField] private Sprite imgOnMusic, imgOffMusic;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        LoadData.EndGame = false;
    }

    private void Start()
    {
        NewGame();
        if (LoadData.IsTimeDown)
        {
            sliderTimeUI.SetActive(true);
            StartCoroutine(Timer());
        }
        else
        {
            sliderTimeUI.SetActive(false);
        }
    }

    private void NewGame()
    {
        LoadData.EndGame = false;
        width = LoadData.Row;
        height = LoadData.Col;
        mineCount = LoadData.Mine;

        state = new Cell[width, height];
        GenerateCells();
        GenerateMines();
        GenerateNumbers();

        Camera.main.transform.position = new Vector3(width / 2f, 4f * Mathf.Sqrt(width * height) + 10f,/*height / 2f + */0f);
        Map.position = new Vector3(width / 2f, -1.5f, height / 2f);
        Map.localScale = Vector3.one * Mathf.Max(height, width) /1.7f;
        board.Draw(state);
    }

    private void GenerateCells()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (state[x, y].type == Cell.Type.Mine)
            {
                x++;

                if (x >= width)
                {
                    x = 0;
                    y++;

                    if (y >= height) {
                        y = 0;
                    }
                }
            }

            state[x, y].type = Cell.Type.Mine;
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine) {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0) {
                    cell.type = Cell.Type.Number;
                }

                state[x, y] = cell;
            }
        }
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjacentX = -1; adjacentX <= 1; adjacentX++)
        {
            for (int adjacentY = -1; adjacentY <= 1; adjacentY++)
            {
                if (adjacentX == 0 && adjacentY == 0) {
                    continue;
                }

                int x = cellX + adjacentX;
                int y = cellY + adjacentY;

                if (GetCell(x, y).type == Cell.Type.Mine) {
                    count++;
                }
            }
        }

        return count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            NewGame();
        }
        else if (!LoadData.EndGame)
        {
            if (Input.GetMouseButtonDown(1)) {
                Flag();
            } else if (Input.GetMouseButtonDown(0)) {
                Reveal();
            }
        }else if (LoadData.EndGame)
        {
            Invoke("LoadEndGameUI", 1.5f);
        }

        txtFlag.SetText(flag + "/" + mineCount);

    }

    public IEnumerator Timer()
    {
        timeDown = LoadData.Time;
        sliderTime.maxValue = timeDown;
        do
        {
            sliderTime.value = timeDown;
            yield return new WaitForSeconds(1);
            if (timeDown < 0)
            {
                LoadData.EndGame = true;
                LoadData.WinGame = false;
                break;
            }
        } while (timeDown-- >= 0 && !LoadData.EndGame);
    }

    public static Vector3 GetMouseWorldPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hit))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void Flag()
    {
        Vector3 worldPosition = GetMouseWorldPosition();
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        // Cannot flag if already revealed
        if (cell.type == Cell.Type.Invalid || cell.revealed) {
            return;
        }

        cell.flagged = !cell.flagged;
        if (cell.flagged == true) flag++;
        else flag--;
        state[cellPosition.x, cellPosition.y] = cell;
        board.Draw(state);
    }

    private void Reveal()
    {
        Vector3 worldPosition = GetMouseWorldPosition();
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        // Cannot reveal if already revealed or while flagged of endgame
        if (cell.type == Cell.Type.Invalid || cell.revealed || cell.flagged || LoadData.EndGame) {
            return;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine:
                Explode(cell);
                break;

            case Cell.Type.Empty:
                Flood(cell);
                CheckWinCondition();
                break;

            default:
                cell.revealed = true;
                state[cellPosition.x, cellPosition.y] = cell;
                CheckWinCondition();
                break;
        }

        board.Draw(state);
    }

    private void Flood(Cell cell)
    {
        // Recursive exit conditions
        if (cell.revealed) return;
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid) return;

        // Reveal the cell
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        // Keep flooding if the cell is empty, otherwise stop at numbers
        if (cell.type == Cell.Type.Empty)
        {
            Flood(GetCell(cell.position.x - 1, cell.position.y));
            Flood(GetCell(cell.position.x + 1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y - 1));
            Flood(GetCell(cell.position.x, cell.position.y + 1));
        }
    }

    public void LoadEndGameUI()
    {
        SceneManager.LoadSceneAsync(2);
    }

    private void Explode(Cell cell)
    {
        Debug.Log("Game Over!");
        LoadData.WinGame = false;
        LoadData.EndGame = true;

        // Set the mine as exploded
        cell.exploded = true;
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;

        // Reveal all other mines
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed) {

                    return;
                }
            }
        }

        Debug.Log("Winner!");
        LoadData.EndGame = true;
        LoadData.WinGame = true;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private Cell GetCell(int x, int y)
    {
        if (IsValid(x, y)) {
            return state[x, y];
        } else {
            return new Cell();
        }
    }

    private bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public void _btnPause()
    {
        popUpPasue.SetActive(true);
        Time.timeScale = 0;
    }

    public void _btnQuit()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void _btnResume()
    {
        popUpPasue.SetActive(false);
        Time.timeScale = 1;
    }

    public void _btnMute()
    {
        LoadData.IsMute = !LoadData.IsMute;
        if (!LoadData.IsMute)
        {
            imgBtnMute.sprite = imgOnMusic;
        }
        else
        {
            imgBtnMute.sprite = imgOffMusic;
        }
    }
}
