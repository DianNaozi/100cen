using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManger : MonoBehaviour
{
    /**
     * 生成阶梯
     */
    [SerializeField] GameObject[] FloorPrefabs;
    
    public void SpwanFloor()
    {
        int range = Random.Range(0, FloorPrefabs.Length);
        GameObject floor = Instantiate(FloorPrefabs[range],transform);
        floor.transform.position = new Vector3(Random.Range(-3.8f,3.8f), -6f, 0);
    }
}
