using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnDeath;
    public bool IsDead { get; private set; }
    public float DeathSightHeight = 0.3f;

    private PlayerMovement _movement;
    private PlayerSetWard _setWard;
    private Vector3 _playerInitialPosition;
    private void Awake()
    {
        IsDead = false;
        _movement = GetComponent<PlayerMovement>();
        _setWard = GetComponent<PlayerSetWard>();
        _playerInitialPosition = transform.position;
    }

    private void OnEnable()
    {
        _movement.enabled = true;
        _setWard.enabled = true;
    }

    public void Die()
    {
        OnDeath?.Invoke();
        IsDead = true;

        GameManager.Instance.End();
        // gameObject.SetActive(false);
        _movement.enabled = false;
        _setWard.enabled = false;
    }

    public void Revive()
    {
        transform.position = _playerInitialPosition;
        transform.rotation = Quaternion.identity;
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
}