using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : EventUI
{
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _Clip;
    protected new void Awake()
    {
       base.Awake();
        _audio = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _childs[i].SetActive(true);
        }
        _audio?.PlayOneShot(_Clip);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.AddListener(Activate);
    }

    private new void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.OnGameOver.RemoveListener(Activate);
    }
}
