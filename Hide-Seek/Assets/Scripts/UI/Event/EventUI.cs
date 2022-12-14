using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    protected GameObject[] _childs;
    protected Button _restartBtn;
    protected Button _titleBtn;
    protected Button _quitBtn;
    
    protected void Awake()
    {
        _childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i] = transform.GetChild(i).gameObject;
            _childs[i].SetActive(false);
        }

        _restartBtn = transform.Find("RestartButton").GetComponent<Button>();
        _titleBtn = transform.Find("TitleButton").GetComponent<Button>();
        _quitBtn = transform.Find("QuitButton").GetComponent<Button>();

        _restartBtn.onClick.AddListener(Restart);
        _titleBtn.onClick.AddListener(LoadTitle);
        _quitBtn.onClick.AddListener(Quit);
    }

    public void Restart() => GameManager.Instance.Restart();
    public void LoadTitle() => GameManager.Instance.LoadTitle();
    public void Quit() => GameManager.Instance.Quit();

    protected void OnDisable()
    {
        _restartBtn.onClick.RemoveListener(Restart);
        _titleBtn.onClick.RemoveListener(LoadTitle);
        _quitBtn.onClick.RemoveListener(Quit);
    }
}
