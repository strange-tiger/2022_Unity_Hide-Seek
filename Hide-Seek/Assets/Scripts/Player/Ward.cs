using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : Sight
{
    public float Duration = 10f;
    public float CoolTime = 5f;
    public float SightSize = 0.5f;

    private new void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        transform.localScale = SightSize * Vector3.one;
        StartCoroutine(Hold());
    }

    public IEnumerator Hold()
    {
        yield return new WaitForSeconds(Duration);
        transform.localScale = 0.01f * Vector3.one;
        
        yield return new WaitForSeconds(CoolTime);
        gameObject.SetActive(false);
    }

    public float RotationSpeed = 60f;
    private void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
    }
}
