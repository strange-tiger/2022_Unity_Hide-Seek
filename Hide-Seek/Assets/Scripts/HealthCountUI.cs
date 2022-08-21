using UnityEngine;
using TMPro;

public class HealthCountUI : MonoBehaviour
{
    private TextMeshProUGUI _ui;
    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
        UpdateText(3);
    }

    public void UpdateText(int health) => _ui.text = $"X {health}";

    void OnEnable()
    {
        GameManager.Instance.OnHealthChanged.AddListener(UpdateText);
    }

    void OnDisable()
    {
        GameManager.Instance.OnHealthChanged.RemoveListener(UpdateText);
    }
}
