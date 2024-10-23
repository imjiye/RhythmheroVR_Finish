using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CubeNote : MonoBehaviour
{
    public static List<CubeNote> allCubes = new List<CubeNote>(); // ������ ��� ť�긦 �����ϱ�

    private bool isSliced = false; // �߸��� true�� ����

    public Vector3 spawnDirection;

    private void Awake()
    {
        allCubes.Add(this); // ť�갡 ������ ������ ����Ʈ�� �߰��ǰ� ��
    }

    private void OnDestroy()
    {
        allCubes.Remove(this); // ť�갡 �ı��� ������ ����Ʈ���� �����ϱ�
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSliced) // �߸��� �ʾ��� ���� �̵�
        {
            transform.position += Time.deltaTime * transform.forward * 2;
        }
    }

    public void SetSliced()
    {
        isSliced = true; // �߸� ���·� ����
    }

    public bool CanSlice(Vector3 sliceDirection)
    {
        // ���� ����� �߸��� ������ ������ ��
        return Vector3.Angle(spawnDirection, sliceDirection) < 45f; // ���� ������ ����
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
        // ����Ʈ�� �ִ� ��� ť�� �����ϱ�
        foreach(CubeNote cube in new List<CubeNote>(allCubes))
        {
            if(cube != null)
            {
                Destroy(cube.gameObject);
            }
        }
    }
}
