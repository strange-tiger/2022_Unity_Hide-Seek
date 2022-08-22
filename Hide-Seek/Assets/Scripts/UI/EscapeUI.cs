using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUI : MonoBehaviour
{
    private GameObject[] _childs;
    private Button _restartBtn;
    private void Awake()
    {
        _childs = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i] = transform.GetChild(i).gameObject;
            _childs[i].SetActive(false);
        }

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

    private void OnEnable()
    {
        GameManager.Instance.OnEscape.AddListener(Activate);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnEscape.RemoveListener(Activate);
        _restartBtn.onClick.RemoveListener(Restart);
    }
}
