using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureDoll : Detectable
{
    [Header("Lure")]
    public float Duration = 3f;
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
    private float _Cooltime = 30f;
    private int _enemyLayer;
    private Collider _lure;
    private MeshRenderer _lureRenderer;
    private new void Awake()
    {
        base.Awake();

        CurrentGauge = Duration;
        _enemyLayer = LayerMask.NameToLayer("Enemy");
        _lure = GetComponent<Collider>();
        _lureRenderer = GetComponent<MeshRenderer>();
        
        gameObject.SetActive(false);
    }

    private void OnUse()
    {
        _lure.enabled = true;
        _lureRenderer.enabled = true;
    }
    private void OnCooltime()
    {
        _lure.enabled = false;
        _lureRenderer.enabled = false;
        marker.SetActive(false);
    }


    private void OnEnable()
    {
        StartCoroutine(Hold());
    }

    public IEnumerator Hold()
    {
        OnUse();
        while (CurrentGauge > 0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge -= Time.deltaTime;
        }

        OnCooltime();
        float coefficient = Duration / _Cooltime;
        while (CurrentGauge < _Cooltime * coefficient)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            CurrentGauge += Time.deltaTime * coefficient;
        }
        CurrentGauge = Duration;

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _enemyLayer)
        {
            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
            enemy.TargetCatched();
        }
    }
}
