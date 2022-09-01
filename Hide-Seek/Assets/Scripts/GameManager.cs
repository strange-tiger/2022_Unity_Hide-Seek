using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent OnEscape = new UnityEvent();
    public UnityEvent<int> OnKeyChanged = new UnityEvent<int>();
    public int CurrentKey
    {
        get
        {
            return _currentKey;
        }
        set
        {
            _currentKey = value;
            OnKeyChanged?.Invoke(_currentKey);
        }
    }
    public UnityEvent AllKeysCollected = new UnityEvent();
    public UnityEvent<int> OnHealthChanged = new UnityEvent<int>();
    public int HealthCount
    {
        get
        {
            return _healthCount;
        }
        set
        {
            _healthCount = value;
            OnHealthChanged?.Invoke(_healthCount);
        }
    }
    public UnityEvent<bool> OnPause = new UnityEvent<bool>();
    public bool IsPause
    {
        get
        {
            return _isPause;
        }
        set
        {
            _isPause = value;
            OnPause?.Invoke(_isPause);
        }
    }
    [Header("Escape Conditions")]
    public int KeyCountMax = 10;
    public int InitHealthCount = 3;

    private int _healthCount;
    private int _currentKey = 0;
    private bool _isGameOver = false;
    private bool _isEscape = false;
    private bool _isPause = false;
    private new void Awake()
    {
        base.Awake();

        SceneManager.activeSceneChanged -= SetVR;
        SceneManager.activeSceneChanged += SetVR;
    }

    private GameObject nonVRobj;
    private GameObject VRobj;
    private StandaloneInputModule nonVREventSystem;
    private OVRInputModule VREventSystem;
    private void SetVR(Scene current, Scene next)
    {
        nonVRobj = null;
        VRobj = null;
        nonVREventSystem = FindObjectOfType<StandaloneInputModule>();
        VREventSystem = FindObjectOfType<OVRInputModule>();

        if (current.name == null)
        {
            Debug.Log("Game Start");
        }

        nonVRobj = next.GetRootGameObjects()[0];
        VRobj = next.GetRootGameObjects()[1];

        checkAndSetChunk(next, nonVRobj, "nonVR");
        Debug.Assert(nonVRobj != null);

        checkAndSetChunk(next, VRobj, "VR");
        Debug.Assert(VRobj != null);

#if UNITY_ANDROID
        nonVRobj.SetActive(false);
        VRobj.SetActive(true);
        nonVREventSystem.enabled = true;
        VREventSystem.enabled = false;
#else
        nonVRobj.SetActive(true);
        VRobj.SetActive(false);
        nonVREventSystem.enabled = true;
        VREventSystem.enabled = false;
#endif
    }

    private void checkAndSetChunk(Scene next, GameObject chunk, string chunkName)
    {
        if (chunk.name != chunkName)
        {
            for (int i = 2; i < next.rootCount; ++i)
            {
                GameObject temp = next.GetRootGameObjects()[i];

                if (temp.name == chunkName)
                {
                    chunk = temp;
                    break;
                }
            }

            chunk = null;
        }
    }

    private void Start()    
    {
        reset();
    }

    private void reset()
    {
        _currentKey = 0;
        _healthCount = InitHealthCount;
        _isGameOver = false;
        _isEscape = false;
        IsPause = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            IsPause = !IsPause;
        }
    }

    public void start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            reset();
            SceneManager.LoadScene(1);
        }
    }

    public void Quit()
    {
        Debug.Log("Quit");

        if (uiExceptionHandling())
        {
            Application.Quit();
            return;
        }
    }

    public void Restart()
    {
        if (uiExceptionHandling())
        {
            reset();
            SceneManager.LoadScene(1);
            return;
        }
    }

    public void LoadTitle()
    {
        if (uiExceptionHandling())
        {
            reset();
            SceneManager.LoadScene(0);
            return;
        }
    }

    private bool uiExceptionHandling()
    {
        if (_isGameOver || _isEscape || IsPause || SceneManager.GetActiveScene().buildIndex == 0)
        {
            return true;
        }

        return false;
    }

    public void AddKey()
    {
        ++CurrentKey;
        if(CurrentKey == KeyCountMax)
        {
            AllKeysCollected?.Invoke();
        }
    }

    public void SubHealth()
    {
        --HealthCount;
    }

    public void End()
    {
        if (HealthCount <= 0)
        {
            _isGameOver = true;
            OnGameOver?.Invoke();
        }
    }

    public void Escape()
    {
        _isEscape = true;
        OnEscape?.Invoke();
    }
}