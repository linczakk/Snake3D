using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartGameCounter : MonoBehaviour
{
    [SerializeField] private int _counterStart = 3;
    [SerializeField] private GameObject _StartGameCounterScreen; 
    [SerializeField] private Text _counterUI;

    public static bool IsGameStarting;

    private void Start()
    {
        StartCoroutine(Wait());
        Time.timeScale = 0.0001f;
        IsGameStarting = true;
    }

    private IEnumerator Wait()
    {
        while (_counterStart >= 0)
        {
            _counterUI.text = _counterStart.ToString();
            yield return new WaitForSecondsRealtime(1);
            _counterStart--;
            AudioManager.Instance.MakeTickSound();
        }
        _StartGameCounterScreen.SetActive(false);
        Time.timeScale = 1f;
        IsGameStarting = false;
    }
}

