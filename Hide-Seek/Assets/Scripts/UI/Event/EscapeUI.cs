using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUI : EventUI
{
    private GameObject _noticeGoalTxt;
    private GameObject _noticePortalTxt;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _Clip;
    private new void Awake()
    {
        base.Awake();
        _audio = GetComponent<AudioSource>();

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

    public void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i].SetActive(true);
        }
        _audio?.PlayOneShot(_Clip);

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

    private new void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.OnEscape.RemoveListener(Activate);
        GameManager.Instance.AllKeysCollected.RemoveListener(NoticePortal);
    }
}
