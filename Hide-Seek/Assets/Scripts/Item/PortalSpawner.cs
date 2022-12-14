using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PortalSpawner : MonoBehaviour
{
    public int PositionRange;

    private NavMeshAgent _navMeshAgent;
    private GameObject _portal;
    private float _floatHeight;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;
        _portal = transform.GetChild(0).gameObject;
        _portal.SetActive(false);
        _floatHeight = _portal.transform.position.y;
    }

    public void OpenPortal()
    {
        int x;
        int y;
        Vector3 positionCandidate;
        NavMeshPath path = new NavMeshPath();
        do
        {
            x = Random.Range(2, PositionRange - 1);
            y = Random.Range(2, PositionRange - 1);

            positionCandidate = new Vector3(x, _floatHeight, y);
        }
        while (!_navMeshAgent.CalculatePath(positionCandidate, path));

        _portal.transform.position = positionCandidate;
        _portal.SetActive(true);
    }

    private void OnEnable()
    {
        GameManager.Instance.AllKeysCollected.AddListener(OpenPortal);
    }

    private void OnDisable()
    {
        GameManager.Instance.AllKeysCollected.RemoveListener(OpenPortal);
    }
}
