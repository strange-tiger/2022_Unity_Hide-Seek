using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LureCooltimeUI : MonoBehaviour
{
    public LureDoll lure;

    private Slider _cooltimeSlider;
    private void Awake()
    {
        _cooltimeSlider = GetComponentInChildren<Slider>();
        _cooltimeSlider.maxValue = lure.Duration;
        UpdateGauge(_cooltimeSlider.maxValue);

        lure.GaugeChanged -= UpdateGauge;
        lure.GaugeChanged += UpdateGauge;
    }

    public void UpdateGauge(float gauge)
    {
        _cooltimeSlider.value = gauge;
    }
}
