using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public PlayerInput input;

    private GameObject _miniMap;
    private GameObject _fullMap;
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            if (child.name == "MiniMap")
            {
                _miniMap = child.gameObject;
            }
            else if (child.name == "FullMap")
            {
                _fullMap = child.gameObject;
            }
        }
        MapToggle(false);

        input.OnFullMap -= MapToggle;
        input.OnFullMap += MapToggle;
    }

    public void MapToggle(bool onFull)
    {
        _miniMap.SetActive(!onFull);
        _fullMap.SetActive(onFull);
    }
}
