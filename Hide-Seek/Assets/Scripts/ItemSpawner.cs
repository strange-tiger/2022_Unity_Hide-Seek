using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum ItemIndex
{
    Key,
    // Ward,
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

        int count = 0;
        _positions = new Vector3[ItemKind][];
        for (int j = 0; j < ItemKind; ++j)
        {
            _positions[j] = new Vector3[ItemCount[j]];

            for (int i = 0; i < ItemCount[j]; ++i)
            {
                int x;
                int y;
                Vector3 positionCandidate;

                do
                {
                    do
                    {
                        x = Random.Range(2, PositionRange - 1);
                        y = Random.Range(2, PositionRange - 1);
                        count++;
                        Debug.Assert(count < 50);
                    }
                    while (_positionUsedArr[y, x] == true);
                    _positionUsedArr[y, x] = true;
                    positionCandidate = new Vector3(x, FloatHeight, y);

                    _navMeshAgent.SetDestination(positionCandidate);
                }
                while (_navMeshAgent.destination != transform.position);

                _positions[j][i] = positionCandidate;
            }
        }
    }
}
