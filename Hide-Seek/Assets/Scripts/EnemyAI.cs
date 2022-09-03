using System;
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
    // 상태
    [Header("State")]
    [SerializeField]
    private EnemyState _State;
    [SerializeField]
    private EnemyState _PrevState = EnemyState.None;
    [SerializeField]
    private float _IdleTime = 2f;
    [SerializeField]
    private float _WalkTime = 5f;
    [SerializeField]
    private float _CatchTime = 5f;

    // 이동
    [Header("Move")]
    [SerializeField]
    private float _WalkSpeed = 1f;
    [SerializeField]
    private float _RunSpeed = 8f;
    [SerializeField]
    private Vector3 _InitPosition = Vector3.zero;
    private Rigidbody _rigidbody;

    // 추적
    public static Action TargetDeath;
    [Header("Target to Chase")]
    [SerializeField]
    private LayerMask _TargetLayer;


    private Transform _target;
    private NavMeshAgent _navMeshAgent;

    // 연출
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _targetGameOver = false;
    private new void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        // _audioSource = GetComponent<AudioSource>();

        base.Awake();

        TargetDeath -= backToPosition;
        TargetDeath += backToPosition;

        if(_InitPosition == Vector3.zero)
        {
            _InitPosition = transform.position;
        }

        ChangeState(EnemyState.Idle);
    }
       
    private void Start()
    {
        _navMeshAgent.speed = _WalkSpeed;
    }

    private void Update()
    {
        switch (_State)
        {
            case EnemyState.Idle:   UpdateIdle(); break;
            case EnemyState.Walk:   UpdateWalk(); break;
            case EnemyState.Run:    UpdateRun(); break;
            case EnemyState.Catch:  UpdateCatch(); break;
        }
    }
    private void ChangeState(EnemyState nextState)
    {
        StopAllCoroutines();
        _navMeshAgent.isStopped = true;

        _PrevState = _State;
        _State = nextState;

        _animator.SetBool(EnemyAnimID.IsIdle, false);
        _animator.SetBool(EnemyAnimID.IsWalk, false);
        _animator.SetBool(EnemyAnimID.IsRun, false);
        _animator.SetBool(EnemyAnimID.IsCatch, false);

        switch (_State)
        {
            case EnemyState.Idle:   StartCoroutine(CoroutineIdle()); break;
            case EnemyState.Walk:   StartCoroutine(CoroutineWalk()); break;
            case EnemyState.Run:    StartCoroutine(CoroutineRun()); break;
            case EnemyState.Catch:  StartCoroutine(CoroutineCatch()); break;
        }
    }
#region UpdateDetail
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
        if (_elapsedTime < 0.1f)
        {
            _elapsedTime += Time.deltaTime;
            return;
        }
        _elapsedTime = 0f;
        
        if (!FindTarget())
        {
            ChangeState(EnemyState.Idle);

            return;
        }
    }
    void UpdateCatch()
    {
        // transform.LookAt(_target);
    }
#endregion
#region CoroutineDetail
    IEnumerator CoroutineIdle()
    {
        _animator.SetBool(EnemyAnimID.IsIdle, true);

        while (true)
        {
            yield return new WaitForSeconds(_IdleTime);
            
            ChangeState(EnemyState.Walk);
        }
    }
    IEnumerator CoroutineWalk()
    {
        _animator.SetBool(EnemyAnimID.IsWalk, true);

        Vector3 destination = (_WalkTime * _WalkSpeed) * transform.forward + transform.position;
        
        _navMeshAgent.speed = _WalkSpeed;
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(destination);
        
        if(_navMeshAgent.destination == transform.position)
        {
            _navMeshAgent.isStopped = true;
            float rotateDirection = 90f * Mathf.Pow(-1, UnityEngine.Random.Range(0, 2));

            transform.rotation *= Quaternion.Euler(0f, rotateDirection, 0f);
            ChangeState(EnemyState.Idle);
        }

        while (true)
        {
            yield return new WaitForSeconds(_WalkTime);

            ChangeState(EnemyState.Idle);
        }
    }
    private IEnumerator CoroutineRun()
    {
        _animator.SetBool(EnemyAnimID.IsRun, true);

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _RunSpeed;
        while(true)
        {
            _navMeshAgent.SetDestination(_target.position);
            
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator CoroutineCatch()
    {
        _animator.SetBool(EnemyAnimID.IsCatch, true);
        _navMeshAgent.isStopped = true;

        while (true)
        {
            yield return new WaitForSeconds(_CatchTime);

            if (_target.CompareTag("Player"))
            {
                TargetDeath?.Invoke();
            }
            ChangeState(EnemyState.Idle);
        }
    }
#endregion

    private Collider[] _targetCandidates = new Collider[5];
    private int _targetCandidateCount;
    private bool FindTarget()
    {
        if (_targetGameOver)
        {
            return false;
        }
        
        float minDistance = 101f;
        float distance;
        int targetCandidateCount = Physics.OverlapSphereNonAlloc(transform.position, 10f, _targetCandidates, _TargetLayer);
        for (int i = 0; i < targetCandidateCount; ++i)
        {
            Collider targetCandidate = _targetCandidates[i];

            Debug.Assert(targetCandidate != null);
            if (targetCandidate == null)
            {
                return false;
            }
            
            distance = (targetCandidate.transform.position - transform.position).magnitude;
            if (minDistance > distance)
            {
                minDistance = distance;
                _target = targetCandidate.GetComponent<Transform>();
            }
        }

        if (targetCandidateCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TargetCatched()
    {
        ChangeState(EnemyState.Catch);
    }
    
    private void backToPosition()
    {
        if (_targetGameOver)
        {
            return;
        }
        
        transform.position = _InitPosition;
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
        //if (GameManager.Instance != null) GameManager.Instance.OnGameOver.RemoveListener(TargetGameOver);
        //if (GameManager.Instance != null) GameManager.Instance.OnEscape.RemoveListener(TargetEscaped);
    }

    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
    private new void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
