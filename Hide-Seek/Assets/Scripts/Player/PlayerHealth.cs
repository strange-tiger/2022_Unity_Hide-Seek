using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float DeathSightHeight = 0.3f;
    public event Action OnDeath;
    public bool IsDead { get; private set; }

    private PlayerMovement _movement;
    private void Awake()
    {
        IsDead = false;
        _movement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        _movement.enabled = true;
    }

    public void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;

        // gameObject.SetActive(false);
        _movement.enabled = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.LookAt(collision.transform.position + DeathSightHeight * Vector3.up);
            // Debug.Log("Ouch");
            
            Die();
            // Debug.Log("captured");
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Enemy")
    //    {
    //        Die();
    //        transform.LookAt(other.transform.position + Vector3.up);
    //    }
    //}
}
