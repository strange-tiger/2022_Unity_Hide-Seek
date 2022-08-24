using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : GameOverUI
{
    private new void Awake()
    {
        base.Awake();
    }
    
    public void Toggle(bool isPause)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i].SetActive(isPause);
        }

        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnPause.AddListener(Toggle);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPause.RemoveListener(Toggle);
        _restartBtn.onClick.RemoveListener(Restart);
        _titleBtn.onClick.RemoveListener(LoadTitle);
        _quitBtn.onClick.RemoveListener(Quit);
    }
}
