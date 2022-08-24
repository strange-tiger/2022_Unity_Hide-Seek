using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : Sight
{
    public float SightSize = 0.5f;
    public float Duration = 10f;
    public float Cooltime = 5f;
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
    private new void Awake()
    {
        base.Awake();
        CurrentGauge = Duration;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.localScale = SightSize * Vector3.one;
        StartCoroutine(Hold());
    }

    public IEnumerator Hold()
    {
        while (CurrentGauge > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge -= Time.deltaTime;
        }
        transform.localScale = 0.01f * Vector3.one;

        int coefficient = (int)(Duration / Cooltime);
        
        while (CurrentGauge < Cooltime * coefficient)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge += Time.deltaTime * Duration / Cooltime;
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
