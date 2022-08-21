using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : Sight
{
    public float Duration = 10f;
    public float CoolTime = 5f;

    private void OnEnable()
    {
        transform.localScale = 0.5f * Vector3.one;
        StartCoroutine(Hold());
    }

    public IEnumerator Hold()
    {
        yield return new WaitForSeconds(Duration);
        transform.localScale = 0.01f * Vector3.one;
        
        yield return new WaitForSeconds(CoolTime);
        gameObject.SetActive(false);
    }
}
