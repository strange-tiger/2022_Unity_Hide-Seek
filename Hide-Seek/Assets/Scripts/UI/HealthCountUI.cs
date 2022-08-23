using System.Collections;
using UnityEngine;
using TMPro;

public class HealthCountUI : MonoBehaviour
{
    private TextMeshProUGUI _ui;
    private int _initHealth;
    private int _gameOverHealth;
    private void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
        _initHealth = GameManager.Instance.InitHealthCount;
        _gameOverHealth = 0;
        //_ui.text = $"X {_initHealth}";
    }

    public void UpdateText(int health)
    {
        if (health < _gameOverHealth)
        {
            return;
        }
        
        _ui.text = $"X {health}";
        if (health == _initHealth || health == _gameOverHealth)
        { 
            return;
        }
        StartCoroutine(ChangeSize());
    }

    public float BigSize = 3f;
    public float MoveDownLength = 200f;
    public IEnumerator ChangeSize()
    {
        float changeBiggerTime = 1f;
        float waitTime = 1f;
        float changeSmallerTime = 1f;
        float deltaTime = 0.01f;

        Vector3 initPosition = _ui.transform.localPosition;
        while (changeBiggerTime > 0f)
        {
            _ui.transform.localScale += BigSize * deltaTime * Vector3.one;
            _ui.transform.localPosition -= new Vector3(0f, MoveDownLength * deltaTime);

            changeBiggerTime -= deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }

        yield return new WaitForSeconds(waitTime);

        while (changeSmallerTime > 0f)
        {
            _ui.transform.localScale -= BigSize * deltaTime * Vector3.one;
            _ui.transform.localPosition += new Vector3(0f, MoveDownLength * deltaTime);

            changeSmallerTime -= deltaTime;
            yield return new WaitForSeconds(deltaTime);
        }
        _ui.transform.localPosition = initPosition;
    }

    void OnEnable()
    {
        GameManager.Instance.OnHealthChanged.AddListener(UpdateText);
    }

    void OnDisable()
    {
        GameManager.Instance.OnHealthChanged.RemoveListener(UpdateText);
    }
}
