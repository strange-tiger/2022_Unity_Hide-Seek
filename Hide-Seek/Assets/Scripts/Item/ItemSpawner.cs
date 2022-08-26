using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ItemIndex
{
    Key,
    Sonar,
    Max
}

public class ItemSpawner : MonoBehaviour
{
    [Header("Items")]
    public GameObject[] ItemKinds;

    private NavMeshAgent _navMeshAgent;
    private GameObject[][] _items;
    private Vector3[][] _positions;
    private bool[,] _positionUsedArr;
    [SerializeField]
    private int[] _ItemCount;

    [Header("Positions")]
    [SerializeField]
    private int _PositionRange;
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;

        if (ItemKinds[(int)ItemIndex.Key] != null)
        {
            _ItemCount[(int)ItemIndex.Key] = GameManager.Instance.KeyCountMax;
        }

        generateItems();
    }

    private void generateItems()
    {
        Debug.Assert(ItemKinds.Length == (int)ItemIndex.Max);
        Debug.Assert(ItemKinds.Length == _ItemCount.Length);
        int itemKind = (int)ItemIndex.Max;

        setItemPositionArr(itemKind);

        _items = new GameObject[itemKind][];
        for (int j = 0; j < itemKind; ++j)
        {
            _items[j] = new GameObject[_ItemCount[j]];
            for (int i = 0; i < _ItemCount[j]; ++i)
            {
                _items[j][i] = Instantiate(ItemKinds[j], transform.position + _positions[j][i], Quaternion.identity);
                _items[j][i].transform.SetParent(transform);
                _items[j][i].SetActive(true);
            }
        }
    }

    private void setItemPositionArr(int ItemKind)
    {
        _positionUsedArr = new bool[_PositionRange, _PositionRange];
        for (int j = 0; j < _PositionRange; ++j)
        {
            for (int i = 0; i < _PositionRange; ++i)
            {
                _positionUsedArr[j, i] = false;
            }
        }

        _positions = new Vector3[ItemKind][];
        for (int j = 0; j < ItemKind; ++j)
        {
            _positions[j] = new Vector3[_ItemCount[j]];

            int x;
            int y;
            Vector3 positionCandidate;
            for (int i = 0; i < _ItemCount[j]; ++i)
            {
                x = Random.Range(2, _PositionRange - 1);
                y = Random.Range(2, _PositionRange - 1);

                positionCandidate = new Vector3(x, 0f, y);
                if (_positionUsedArr[y, x] == true)
                {
                    --i;
                    Debug.Log("Recall");
                    continue;
                }
                markUsedArea(y, x);

                NavMeshPath path = new NavMeshPath();
                if (!_navMeshAgent.CalculatePath(positionCandidate, path))
                {
                    --i;
                    Debug.Log("Recallnav");
                    continue;
                }
                _positions[j][i] = positionCandidate;
            }
        }
    }
    [Header("Set Min Distance Between Items")]
    [SerializeField]
    private int _UsedArea = 10;
    private void markUsedArea(int y, int x)
    {
        for (int j = -_UsedArea; j <= _UsedArea; ++j)
        {
            for (int i = -_UsedArea; i <= _UsedArea; ++i)
            {
                if (y + j <= 2 || y + j >= _PositionRange - 1)
                {
                    continue;
                }
                if (x + i <= 2 || x + i >= _PositionRange - 1)
                {
                    continue;
                }

                _positionUsedArr[y + j, x + i] = true;
            }
        }
    }
}
