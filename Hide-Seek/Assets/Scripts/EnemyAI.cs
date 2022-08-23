using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    None,
    Idle,
    Walk,
    Catch,
    Run
}

public static class EnemyAnimID
{
    public static readonly int IsIdle = Animator.StringToHash("IsIdle");
    public static readonly int IsWalk = Animator.StringToHash("IsWalk");
    public static readonly int IsRun = Animator.StringToHash("IsRun");
    public static readonly int IsCatch = Animator.StringToHash("IsCatch");
}

public class EnemyAI : Detectable
{
    public LayerMask TargetLayer;
    public EnemyState State;
    public EnemyState PrevState = EnemyState.None;
    public float WalkSpeed = 1f;
    public float RunSpeed = 7f;
    public float IdleTime = 2f;
    public float WalkTime = 5f;
    public float CatchTime = 5f;

    private Transform _target;
    private NavMeshAgent _navMeshAgent;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    private Renderer _renderer;

    private bool _targetGameOver = false;
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

    private new void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        // _audioSource = GetComponent<AudioSource>();
        // _renderer = GetComponent<Renderer>();

        base.Awake();

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
            case EnemyState.Catch: UpdateCatch(); break;
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
        _elapsedTime = 0f;

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
        _elapsedTime = 0f;

        if (FindTarget())
        {
            ChangeState(EnemyState.Run);

            return;
        }
    }
    void UpdateRun()
    {
        
    }
    void UpdateCatch()
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
        _animator.SetBool(EnemyAnimID.IsCatch, false);

        switch (State)
        {
            case EnemyState.Idle: StartCoroutine(CoroutineIdle()); break;
            case EnemyState.Walk: StartCoroutine(CoroutineWalk()); break;
            case EnemyState.Run: StartCoroutine(CoroutineRun()); break;
            case EnemyState.Catch: StartCoroutine(CoroutineCatch()); break;
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

        Vector3 destination = (WalkTime * WalkSpeed) * transform.forward + transform.position;
        
        _navMeshAgent.speed = WalkSpeed;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(destination);
        
        if(_navMeshAgent.destination == transform.position)
        {
            _navMeshAgent.isStopped = true;

            transform.rotation *= Quaternion.Euler(0f, Random.Range(-1, 2) * 90f, 0f);
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
    IEnumerator CoroutineCatch()
    {
        _animator.SetBool(EnemyAnimID.IsCatch, true);

        while (true)
        {
            yield return new WaitForSeconds(CatchTime);

            ChangeState(EnemyState.Idle);
        }
    }

    private Collider[] _targetCandidates = new Collider[5];
    private int _targetCandidateCount;
    private bool FindTarget()
    {
        _targetCandidateCount = Physics.OverlapSphereNonAlloc(transform.position, 20f, _targetCandidates, TargetLayer);

        for (int i = 0; i < _targetCandidateCount; ++i)
        {
            Collider targetCandidate = _targetCandidates[i];

            Debug.Assert(targetCandidate != null);
            if (targetCandidate != null && !_targetGameOver)
            {
                _target = targetCandidate.GetComponent<Transform>(); ;
                PlayerHealth playerHealth = _target.GetComponent<PlayerHealth>();

                playerHealth.OnDeath -= this.TargetCatched;
                playerHealth.OnDeath += this.TargetCatched;

                return true;
            }
        }

        return false;
    }

    public void TargetCatched()
    {
        Debug.Log("Catch");
        ChangeState(EnemyState.Catch);
    }

    public void TargetGameOver() => _targetGameOver = true;
    public void TargetEscaped() => gameObject.SetActive(false);
    private void OnEnable()
    {
        GameManager.Instance.OnGameOver.AddListener(TargetGameOver);
        GameManager.Instance.OnEscape.AddListener(TargetEscaped);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver.RemoveListener(TargetGameOver);
        GameManager.Instance.OnEscape.RemoveListener(TargetEscaped);
    }

    private new void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
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
