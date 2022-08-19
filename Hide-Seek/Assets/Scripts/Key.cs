using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    private void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameObject.SetActive(false);
        }
    }
}
