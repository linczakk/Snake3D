using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    //Handling pickups
    public static GameplayController Instance;

    [SerializeField]
    private GameObject _bombPickUp;
    [SerializeField]
    private GameObject[] _foodPickUps;

    private float _min_X = -2.7f, _max_X = 2.7f, _min_Y = -2.25f, _max_Y = 2.26f;
    private float _z_Pos = 8.875f;
    

    //Handling gameover
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Text _scoreGoUI;
    [SerializeField] private GameObject _newBestScoreHeader;
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private Text _bestScoreTextUI;
    private bool _gameOver;

        

    //Handling help panel
    [SerializeField] private GameObject _helpPanel;

    //Handling no sounds icon
    [SerializeField] private GameObject _disabledSoundUI;

    private bool _disableSounds;

    private void Awake()
    {
        _disableSounds = !AudioController._audioOff;
        MakeInstance();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Invoke(nameof(StartSpawning), 0.05f);
    }

    private void Update()
    {
        if(_gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        HelpWindow();
        SoundsControl();
    }

    private void StartSpawning()
    {
        StartCoroutine(SpawnPickUps());
    }

    public void CancelSpawning()
    {
        CancelInvoke(nameof(StartSpawning));
    }

    private void MakeInstance()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private IEnumerator SpawnPickUps()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        if(Random.Range(0, 10) >= 2)
        {
            GetRandomPositionToSpawn(RandomPickUp(), Quaternion.Euler(-90f, 0f, 0f));
        }
        else
        {
            GetRandomPositionToSpawn(_bombPickUp, Quaternion.identity);
            AudioManager.Instance.RespawnBombSound();
        }
        Invoke(nameof(StartSpawning), 0f);
    }
    private GameObject RandomPickUp() => _foodPickUps[Random.Range(0, _foodPickUps.Length)];

    private void GetRandomPositionToSpawn(GameObject pickUp, Quaternion quaternion) => Instantiate(pickUp, new Vector3(Random.Range(_min_X, _max_X),
                                                                                                Random.Range(_min_Y, _max_Y),
                                                                                                _z_Pos), quaternion);
    public void OnGameOver()
    {
        _inGameUI.SetActive(false);
        _gameOverScreen.SetActive(true);
        _scoreGoUI.text = ScoreController.Instance.GetScoreCount().ToString();
        _gameOver = true;

        ScoreController.Instance.SetBestScore(_newBestScoreHeader, _bestScoreTextUI);

        Time.timeScale = 0f;
        AudioManager.Instance.PlayDeadSound();
    }
    private void HelpWindow()
    {
        if (Input.GetKeyDown(ControllsManager.GetHelpWindow()))
        {
            _helpPanel.SetActive(true);
            if (!_gameOver && !StartGameCounter.IsGameStarting)
            {
                Time.timeScale = 0.0001f;
            }
        }
        if (Input.GetKeyUp(ControllsManager.GetHelpWindow()))
        {
            _helpPanel.SetActive(false);
            if (!_gameOver && !StartGameCounter.IsGameStarting)
            {
                Time.timeScale = 1f;
            }
        }
        if (Input.GetKey(ControllsManager.ExitGameKey()))
        {
            Application.Quit();
        }
    }

    private void SoundsControl()
    {
        if(Input.GetKeyDown(ControllsManager.TurnOffOnSounds()))
        {
            _disableSounds = !_disableSounds;
            AudioManager.Instance.ShouldTurnOffSound(_disableSounds);
        }
        _disabledSoundUI.SetActive(!AudioController._audioOff);
    }
}

