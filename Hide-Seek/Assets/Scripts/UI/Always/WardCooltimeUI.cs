using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WardCooltimeUI : MonoBehaviour
{
    public Ward ward;

    private Slider _cooltimeSlider;
    private void Awake()
    {
        _cooltimeSlider = GetComponentInChildren<Slider>();
        _cooltimeSlider.maxValue = ward.Duration;
        UpdateGauge(_cooltimeSlider.maxValue);

        ward.GaugeChanged -= UpdateGauge;
        ward.GaugeChanged += UpdateGauge;
    }

    public void UpdateGauge(float gauge)
    {
        _cooltimeSlider.value = gauge;
    }
}
