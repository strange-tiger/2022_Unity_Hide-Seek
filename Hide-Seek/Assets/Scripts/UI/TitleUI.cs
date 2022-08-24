using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    private Button _startBtn;
    private Button _quitBtn;
    private void Awake()
    {
        _startBtn = transform.GetChild(0).GetComponent<Button>();
        _quitBtn = transform.GetChild(1).GetComponent<Button>();

        _startBtn.onClick.AddListener(start);
        _quitBtn.onClick.AddListener(Quit);
    }

    public void start() => GameManager.Instance.start();
    public void Quit() => GameManager.Instance.Quit();

    private void OnDisable()
    {
        _startBtn.onClick.RemoveListener(start);
        _quitBtn.onClick.RemoveListener(Quit);
    }
}
