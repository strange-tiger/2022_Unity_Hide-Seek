using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUI : MonoBehaviour
{
    private GameObject[] _childs;
    private GameObject _noticePortalTxt;
    private Button _restartBtn;
    private int _keyCountMax;
    private void Awake()
    {
        _childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i] = transform.GetChild(i).gameObject;
            _childs[i].SetActive(false);

            if (_childs[i].name == "NoticeText")
            {
                _noticePortalTxt = _childs[i];
            }
        }
        _keyCountMax = GameManager.Instance.KeyCountMax;

        _restartBtn = transform.Find("RestartButton").GetComponent<Button>();
        _restartBtn.onClick.AddListener(Restart);
    }

    public void Restart() => GameManager.Instance.Restart();

    public void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i].SetActive(true);
        }
    }

    public void NoticePortal()
    {
        _noticePortalTxt.SetActive(true);
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
    }
}
