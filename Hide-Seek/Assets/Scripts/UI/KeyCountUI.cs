using UnityEngine;
using TMPro;

public class KeyCountUI : MonoBehaviour
{

    private TextMeshProUGUI _ui;
    private int _keyCountMax;
    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
        _keyCountMax = GameManager.Instance.KeyCountMax;
        UpdateText(0);
    }

    public void UpdateText(int key) => _ui.text = $"{key} / {_keyCountMax}";

    void OnEnable()
    {
        GameManager.Instance.OnKeyChanged.AddListener(UpdateText);
    }

    void OnDisable()
    {
        GameManager.Instance.OnKeyChanged.RemoveListener(UpdateText);
    }
}