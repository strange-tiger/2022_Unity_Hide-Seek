using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnDeath;

    private PlayerMovement _movement;
    private PlayerSetWard _setWard;
    private Vector3 _playerInitialPosition;
    private bool _revivable;
    [SerializeField]
    private float _DeathSightHeight = 0.3f;
    private void Awake()
    {
        _revivable = true;
        _movement = GetComponent<PlayerMovement>();
        _setWard = GetComponent<PlayerSetWard>();
        _playerInitialPosition = transform.position;
    }

    public void Die()
    {
        GameManager.Instance.SubHealth();
        GameManager.Instance.End();
        Revive();
        OnDeath?.Invoke();

        // gameObject.SetActive(false);
        _movement.enabled = false;
        _setWard.enabled = false;
    }

    public void Revive()
    {
        if (!_revivable)
        {
            return;
        }
        StartCoroutine(WaitRevive());
    }
    public IEnumerator WaitRevive()
    {
        yield return new WaitForSeconds(3f);
        _movement.enabled = true;
        _setWard.enabled = true;

        transform.position = _playerInitialPosition;
        transform.rotation = Quaternion.identity;
    }

    public void SetRevivable() => _revivable = false;
    public void SetMovable(bool isPause)
    {
        _movement.enabled = !isPause;
        _setWard.enabled= !isPause;
    }
    private void OnEnable()
    {
        _movement.enabled = true;
        _setWard.enabled = true;
        GameManager.Instance.OnGameOver.AddListener(SetRevivable);
        GameManager.Instance.OnPause.AddListener(SetMovable);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(SetRevivable);
        GameManager.Instance.OnPause.RemoveListener(SetMovable);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            transform.LookAt(collision.transform.position + _DeathSightHeight * Vector3.up);
            // Debug.Log("Ouch");

            Die();
            // Debug.Log("captured");
        }
    }
}