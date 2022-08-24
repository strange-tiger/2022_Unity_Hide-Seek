using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUI : GameOverUI
{
    private GameObject _noticeGoalTxt;
    private GameObject _noticePortalTxt;
    private new void Awake()
    {
        base.Awake();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (_childs[i].name == "GoalText")
            {
                _noticeGoalTxt = _childs[i];
            }
            if (_childs[i].name == "NoticeText")
            {
                _noticePortalTxt = _childs[i];
            }
        }
        _noticeGoalTxt?.SetActive(true);
    }

    public new void Activate()
    {
        base.Activate();
        _noticeGoalTxt?.SetActive(false);
        _noticePortalTxt?.SetActive(false);
    }

    public void NoticePortal()
    {
        _noticeGoalTxt?.SetActive(false);
        _noticePortalTxt?.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnEscape.AddListener(Activate);
        GameManager.Instance.AllKeysCollected.AddListener(NoticePortal);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEscape.RemoveListener(Activate);
        GameManager.Instance.AllKeysCollected.RemoveListener(NoticePortal);
        _restartBtn.onClick.RemoveListener(Restart);
        _titleBtn.onClick.RemoveListener(LoadTitle);
        _quitBtn.onClick.RemoveListener(Quit);
    }
}
