using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : EventUI
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

    private new void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.OnPause.RemoveListener(Toggle);
    }
}
