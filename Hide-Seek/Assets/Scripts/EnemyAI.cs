using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    None,
    Idle,
    Walk,
    Run
}

public static class EnemyAnimID
{
    public static readonly int OnCatch = Animator.StringToHash("OnCatch");
    public static readonly int IsIdle = Animator.StringToHash("IsIdle");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsRun = Animator.StringToHash("IsRun");
}

public class EnemyAI : MonoBehaviour
{
    public LayerMask TargetLayer;
    public LayerMask MarkerLayer;
    public EnemyState State;
    public EnemyState PrevState = EnemyState.None;
    public float WalkSpeed = 1f;
    public float RunSpeed = 5.5f;
    public float IdleTime = 2f;
    public float WalkTime = 3f;

    private Transform _target;
    private GameObject _marker;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    private Renderer _renderer;

    private bool _targetIsDead = false;
    private bool _hasTargetFound = false;
    public bool HasTargetFound
    {
        get
        {
            return _hasTargetFound;
        }

        set
        {
            _hasTargetFound = value;
        }
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        // _audioSource = GetComponent<AudioSource>();
        // _renderer = GetComponent<Renderer>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            if(transform.GetChild(i).gameObject.layer == MarkerLayer.value)
            {
                // Debug.Log("Success");
                _marker = transform.GetChild(i).gameObject;
                _marker.SetActive(false);
            }
        }

        ChangeState(EnemyState.Idle);
    }
       
    private void Start()
    {
        _navMeshAgent.speed = WalkSpeed;
    }

    private void Update()
    {
        switch (State)
        {
            case EnemyState.Idle: UpdateIdle(); break;
            case EnemyState.Walk: UpdateWalk(); break;
            case EnemyState.Run: UpdateRun(); break;
        }
    }

    private float _elapsedTime = 0f;
    void UpdateIdle()
    {
        if (_elapsedTime < 0.1f)
        {
            _elapsedTime += Time.deltaTime;
            return;
        }
        if (FindTarget())
        {
            ChangeState(EnemyState.Run);

            return;
        }
    }
    void UpdateWalk()
    {
        if (_elapsedTime < 0.1f)
        {
            _elapsedTime += Time.deltaTime;
            return;
        }
        if (FindTarget())
        {
            ChangeState(EnemyState.Run);

            return;
        }
    }
    void UpdateRun()
    {
        
    }

    private void ChangeState(EnemyState nextState)
    {
        StopAllCoroutines();
        _navMeshAgent.isStopped = true;

        PrevState = State;
        State = nextState;

        _animator.SetBool(EnemyAnimID.IsIdle, false);
        _animator.SetBool(EnemyAnimID.IsWalk, false);
        _animator.SetBool(EnemyAnimID.IsRun, false);

        switch (State)
        {
            case EnemyState.Idle: StartCoroutine(CoroutineIdle()); break;
            case EnemyState.Walk: StartCoroutine(CoroutineWalk()); break;
            case EnemyState.Run: StartCoroutine(CoroutineRun()); break;
        }
    }

    IEnumerator CoroutineIdle()
    {
        _animator.SetBool(EnemyAnimID.IsIdle, true);

        while (true)
        {
            yield return new WaitForSeconds(IdleTime);
            
            ChangeState(EnemyState.Walk);
        }
    }
    IEnumerator CoroutineWalk()
    {
        _animator.SetBool(EnemyAnimID.IsWalk, true);

        Vector3 destination = Vector3.zero;
        Collider[] destinationCandidates = new Collider[5];
        int destinationCandidateCount = Physics.OverlapSphereNonAlloc(transform.position, 40f, destinationCandidates, TargetLayer);

        for (int i = 0; i < _targetCandidateCount; ++i)
        {
            Collider destinationCandidate = destinationCandidates[i];

            Debug.Assert(destinationCandidate != null);
            if (destinationCandidate != null)
            {
                destination = destinationCandidate.GetComponent<Transform>().position;

                break;
            }
        }

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = WalkSpeed;
        _navMeshAgent.SetDestination(destination);
        if(!_navMeshAgent.hasPath)
        {
            ChangeState(EnemyState.Idle);
        }

        while (true)
        {
            yield return new WaitForSeconds(WalkTime);

            ChangeState(EnemyState.Idle);
        }
    }
    private IEnumerator CoroutineRun()
    {
        _animator.SetBool(EnemyAnimID.IsRun, true);

        while(true)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed = RunSpeed;
            _navMeshAgent.SetDestination(_target.position);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private Collider[] _targetCandidates = new Collider[5];
    private int _targetCandidateCount;
    private bool FindTarget()
    {
        _targetCandidateCount = Physics.OverlapSphereNonAlloc(transform.position, 40f, _targetCandidates, TargetLayer);

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

                return true;
            }
        }

        return false;
    }

    public void TargetIsDead()
    {
        _targetIsDead = true;
        _animator.SetTrigger(EnemyAnimID.OnCatch);
        _navMeshAgent.isStopped = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Sight")
        {
            _marker.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sight")
        {
            _marker.SetActive(false);
        }
    }

    //private void controlMarker()
    //{
    //    if (_marker != null)
    //    {
    //        _marker.transform.position = new Vector3(this.transform.position.x, _marker.transform.position.y, this.transform.position.z);
    //        _marker.transform.forward = this.transform.forward;
    //    }
    //}
}
