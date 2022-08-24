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
    public GameObject[] ItemKinds;
    public int[] ItemCount;
    public int PositionRange;
    public float FloatHeight = 0.5f;

    private NavMeshAgent _navMeshAgent;
    private GameObject[][] _items;
    private Vector3[][] _positions;
    private bool[,] _positionUsedArr; 

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;

        if (ItemKinds[(int)ItemIndex.Key] != null)
        {
            ItemCount[(int)ItemIndex.Key] = GameManager.Instance.KeyCountMax;
        }

        generateItems();
    }

    private void generateItems()
    {
        Debug.Assert(ItemKinds.Length == (int)ItemIndex.Max);
        Debug.Assert(ItemKinds.Length == ItemCount.Length);
        int itemKind = (int)ItemIndex.Max;
        
        setItemPositionArr(itemKind);

        _items = new GameObject[itemKind][];
        for (int j = 0; j < itemKind; ++j)
        {
            _items[j] = new GameObject[ItemCount[j]];
            //Debug.Log(ItemCount[j]);
            //Debug.Log(ItemKinds[j] != null);
            for (int i = 0; i < ItemCount[j]; ++i)
            {
                _items[j][i] = Instantiate(ItemKinds[j], transform.position + _positions[j][i], Quaternion.identity);
                
                _items[j][i].transform.SetParent(transform);
                _items[j][i].SetActive(true);
            }
        }
    }

    private void setItemPositionArr(int ItemKind)
    {
        _positionUsedArr = new bool[PositionRange, PositionRange];
        for (int j = 0; j < PositionRange; ++j)
        {
            for (int i = 0; i < PositionRange; ++i)
            {
                _positionUsedArr[j, i] = false;
            }
        }

        _positions = new Vector3[ItemKind][];
        for (int j = 0; j < ItemKind; ++j)
        {
            _positions[j] = new Vector3[ItemCount[j]];

            int x;
            int y;
            Vector3 positionCandidate;
            for (int i = 0; i < ItemCount[j]; ++i)
            {
                x = Random.Range(2, PositionRange - 1);
                y = Random.Range(2, PositionRange - 1);

                //count++;
                //Debug.Assert(count < 50);

                positionCandidate = new Vector3(x, FloatHeight, y);
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
                    //Debug.Log(_navMeshAgent.destination);
                    continue;
                }

                _positions[j][i] = positionCandidate;
            }
        }
    }

    public int UsedArea = 10;
    private void markUsedArea(int y, int x)
    {
        for (int j = -UsedArea; j <= UsedArea; ++j)
        {
            for (int i = -UsedArea; i <= UsedArea; ++i)
            {
                if (y + j <= 2 || y + j >= PositionRange - 1)
                {
                    continue;
                }
                if (x + i <= 2 || x + i >= PositionRange - 1)
                {
                    continue;
                }

                _positionUsedArr[y + j, x + i] = true;
            }
        }
    }
}
