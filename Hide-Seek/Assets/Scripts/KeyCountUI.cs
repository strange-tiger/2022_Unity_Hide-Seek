using UnityEngine;
using TMPro;

public class KeyCountUI : MonoBehaviour
{
    public int KeyCountMax = 10;

    private TextMeshProUGUI _ui;
    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
        UpdateText(0);
    }

    public void UpdateText(int key) => _ui.text = $"{key} / {KeyCountMax}";

    void OnEnable()
    {
        GameManager.Instance.OnKeyChanged.AddListener(UpdateText);
    }

    void OnDisable()
    {
        GameManager.Instance.OnKeyChanged.RemoveListener(UpdateText);
    }
}