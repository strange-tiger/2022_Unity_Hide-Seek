using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetLure : MonoBehaviour
{
    [SerializeField]
    private GameObject _LureDoll;
    private PlayerInput _input;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        _input.UseLure -= SetLure;
        _input.UseLure += SetLure;
    }

    public void SetLure()
    {
        if (!_LureDoll.activeSelf)
        {
            _LureDoll.transform.position = transform.position;
            _LureDoll.SetActive(true);
        }
    }
}
