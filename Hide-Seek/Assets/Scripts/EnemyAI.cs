using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class EnemyAnimID
{
    public static readonly int FoundTarget = Animator.StringToHash("FoundTarget");
    public static readonly int OnCatch = Animator.StringToHash("OnCatch");
}

public class EnemyAI : MonoBehaviour
{
    public LayerMask TargetLayer;
    public float Speed = 5f;

    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private AudioSource _audioSource;
    private Renderer _renderer;

    private bool _targetIsDead = false;
    private bool _hasTargetFound
    {
        get
        {
            if (_target != null && !_targetIsDead)
            {
                return true;
            }

            return false;
        }
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        // _audioSource = GetComponent<AudioSource>();
        // _renderer = GetComponent<Renderer>();
    }
       
    private void Start()
    {
        _navMeshAgent.speed = Speed;
        StartCoroutine(updatePath());
    }

    private void Update()
    {
        _animator.SetBool(EnemyAnimID.FoundTarget, _hasTargetFound);
    }

    private Collider[] _targetCandidates = new Collider[3];
    private int _targetCandidateCount;
    private IEnumerator updatePath()
    {
        while(true)
        {
            if (_hasTargetFound)
            {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_target.transform.position);
            }
            else
            {
                _navMeshAgent.isStopped = true;

                _targetCandidateCount = Physics.OverlapSphereNonAlloc(transform.position, 20f, _targetCandidates, TargetLayer);

                for (int i = 0; i < _targetCandidateCount; ++i)
                {
                    Collider targetCandidate = _targetCandidates[i];

                    Debug.Assert(targetCandidate != null);
                    if (targetCandidate != null && !_targetIsDead)
                    {
                        _target = targetCandidate.GetComponent<Transform>(); ;
                        PlayerHealth playerHealth = _target.GetComponent<PlayerHealth>();

                        playerHealth.OnDeath -= this.TargetIsDead;
                        playerHealth.OnDeath += this.TargetIsDead;

                        break;
                    }
                }
            }
            
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TargetIsDead()
    {
        _targetIsDead = true;
        _animator.SetTrigger(EnemyAnimID.OnCatch);
    }
}
