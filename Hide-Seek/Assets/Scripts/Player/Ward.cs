using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : Sight
{
    [Header("Ward")]
    public float Duration = 10f;
    public event Action<float> GaugeChanged;
    public float CurrentGauge
    {
        get
        {
            return _currentGauge;
        }
        set
        {
            _currentGauge = value;
            GaugeChanged?.Invoke(_currentGauge);
        }
    }
    private float _currentGauge;

    [SerializeField]
    private float _SightSize = 0.5f;
    [SerializeField]
    private float _Cooltime = 5f;
    private new void Awake()
    {
        base.Awake();
        CurrentGauge = Duration;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        marker.SetActive(input.IsFullMap);
        StartCoroutine(Sight());
    }

    public IEnumerator Sight()
    {
        transform.localScale = 0.01f * Vector3.one;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = _SightSize * Vector3.one;
        while (CurrentGauge > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge -= Time.deltaTime;
        }
        transform.localScale = 0.01f * Vector3.one;

        int coefficient = (int)(Duration / _Cooltime);
        
        while (CurrentGauge < _Cooltime * coefficient)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge += Time.deltaTime * Duration / _Cooltime;
        }
        CurrentGauge = Duration;
        gameObject.SetActive(false);
    }

    public float RotationSpeed = 60f;
    private void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
    }
}
