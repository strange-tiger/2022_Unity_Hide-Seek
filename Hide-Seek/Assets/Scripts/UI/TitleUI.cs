using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private Button _startBtn;
    private Button _quitBtn;
    private Button _howBtn;
    private GameObject _howPanel;
    private void Awake()
    {
        _startBtn = transform.GetChild(0).GetComponent<Button>();
        _quitBtn = transform.GetChild(1).GetComponent<Button>();
        _howBtn = transform.GetChild(2).GetComponent<Button>();
        _howPanel = transform.GetChild(3).gameObject;
        _howPanel.SetActive(false);

        _startBtn.onClick.AddListener(start);
        _quitBtn.onClick.AddListener(Quit);
        _howBtn.onClick.AddListener(ShowHowToPlay);
    }

    public void start() => GameManager.Instance.start();
    public void Quit() => GameManager.Instance.Quit();
    public void ShowHowToPlay() => _howPanel.SetActive(!_howPanel.activeSelf);

    private void OnDisable()
    {
        _startBtn.onClick.RemoveListener(start);
        _quitBtn.onClick.RemoveListener(Quit);
        _howBtn.onClick.RemoveListener(ShowHowToPlay);
    }
}
