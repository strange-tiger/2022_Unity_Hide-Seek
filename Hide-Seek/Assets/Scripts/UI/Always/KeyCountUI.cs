using UnityEngine;
using TMPro;

public class KeyCountUI : MonoBehaviour
{
    private TextMeshProUGUI _ui;
    private AudioSource _audio;
    [SerializeField]
    private AudioClip _GetKeySound;
    private int _keyCountMax;
    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
        _audio = GetComponent<AudioSource>();
        _keyCountMax = GameManager.Instance.KeyCountMax;
        UpdateText(0);
    }

    public void UpdateText(int key) => _ui.text = $"{key} / {_keyCountMax}";
    
    public void PlaySound(int key)
    {
        if (key <= 0)
        {
            return;
        }
        _audio.PlayOneShot(_GetKeySound);
    }

    void OnEnable()
    {
        GameManager.Instance.OnKeyChanged.AddListener(UpdateText);
        GameManager.Instance.OnKeyChanged.AddListener(PlaySound);
    }

    void OnDisable()
    {
        GameManager.Instance.OnKeyChanged.RemoveListener(UpdateText);
        GameManager.Instance.OnKeyChanged.RemoveListener(PlaySound);
    }
}