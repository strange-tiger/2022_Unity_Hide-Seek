using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    private GameObject[] _childs;
    private Button _restartBtn;
    private Button _titleBtn;
    private void Awake()
    {
        _childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i] = transform.GetChild(i).gameObject;
            _childs[i].SetActive(false);
        }

        _restartBtn = transform.Find("RestartButton").GetComponent<Button>();
        _titleBtn = transform.Find("TitleButton").GetComponent<Button>();

        _restartBtn.onClick.AddListener(Restart);
        _titleBtn.onClick.AddListener(LoadTitle);
    }

    public void Restart() => GameManager.Instance.Restart();
    public void LoadTitle() => GameManager.Instance.LoadTitle();
    public void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i].SetActive(true);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.AddListener(Activate);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(Activate);
        _restartBtn.onClick.RemoveListener(Restart);
        _titleBtn.onClick.RemoveListener(LoadTitle);
    }
}
