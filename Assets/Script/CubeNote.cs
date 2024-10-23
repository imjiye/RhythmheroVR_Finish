using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeNote : MonoBehaviour
{
    public static List<CubeNote> allCubes = new List<CubeNote>(); // 생성된 모든 큐브를 추적하기

    private bool isSliced = false; // 잘리면 true로 변경

    public Vector3 spawnDirection;

    private void Awake()
    {
        allCubes.Add(this); // 큐브가 생성될 때마다 리스트에 추가되게 함
    }

    private void OnDestroy()
    {
        allCubes.Remove(this); // 큐브가 파괴될 때마다 리스트에서 제거하기
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSliced) // 잘리지 않았을 때만 이동
        {
            transform.position += Time.deltaTime * transform.forward * 2;
        }
    }

    public void SetSliced()
    {
        isSliced = true; // 잘린 상태로 변경
    }

    public bool CanSlice(Vector3 sliceDirection)
    {
        // 스폰 방향과 잘리는 방향의 각도를 비교
        return Vector3.Angle(spawnDirection, sliceDirection) < 45f; // 각도 범위를 설정
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Reset")
        {
            GameManager.instance.ResetCombo();
            Destroy(gameObject);
        }
        else if(other.tag == "SlicePoint")
        {
            Debug.Log("Point");
        }
    }

    public static void DestroyAllCube()
    {
        // 리스트에 있는 모든 큐브 삭제하기
        foreach(CubeNote cube in new List<CubeNote>(allCubes))
        {
            if(cube != null)
            {
                Destroy(cube.gameObject);
            }
        }
    }
}
