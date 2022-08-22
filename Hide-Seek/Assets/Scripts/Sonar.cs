using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonar : Sight
{
    public float MaxSize = 2.3f;
    public float Duration = 9f;

    private float _duration;
    private float _deltaTime = 0.1f;
    private new void Awake()
    {
        input = FindObjectOfType<PlayerInput>();
        _duration = Duration / 3;
        base.Awake();
    }

    private void OnEnable()
    {
        marker.SetActive(input.IsFullMap);
        transform.localScale = 0.1f * Vector3.one;
        StartCoroutine(UseSonar());
    }

    private float _setTime = 0f;
    public IEnumerator UseSonar()
    {
        float deltaSize = _deltaTime * MaxSize / _duration;
        while (_setTime < _duration)
        {
            transform.localScale += deltaSize * Vector3.one;

            _setTime += _deltaTime;
            yield return new WaitForSeconds(_deltaTime);
        }
        yield return new WaitForSeconds(_duration);
        while (_setTime > 0f)
        {
            transform.localScale -= deltaSize * Vector3.one;

            _setTime -= _deltaTime;
            yield return new WaitForSeconds(_deltaTime);
        }
        _setTime = 0f;

        gameObject.SetActive(false);
    }
}
