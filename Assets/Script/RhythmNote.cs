using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class RhythmNote : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject[] notes;
    public Transform[] pos; // 노드를 스폰할 위치들
    public float detectionThreshold = 0.1f; // 비트 감지를 위한 임계값
    public float spawnInterval = 0.5f; // 노드 스폰 간격
    private float nextSpawnTime;

    void Update()
    {
        float[] spectrum = new float[256];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // 비트 감지
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > detectionThreshold && Time.time >= nextSpawnTime)
            {
                SpawnNote();
                nextSpawnTime = Time.time + spawnInterval;
                break; // 첫 번째 비트 감지 후 루프 종료
            }
        }
    }

    void SpawnNote()
    {
        int i = Random.Range(0, pos.Length);
        GameObject note = Instantiate(notes[Random.Range(0, notes.Length)], pos[i]);

        note.transform.localPosition = Vector3.zero;
        int j = Random.Range(0, pos.Length);
        note.transform.Rotate(transform.forward, 90 * j);

        // 큐브의 방향을 스크립트에 저장
        CubeNote cubeNote = note.GetComponent<CubeNote>();
        if (cubeNote != null)
        {
            cubeNote.spawnDirection = note.transform.forward; // 큐브의 스폰 방향 저장
        }
    }
}
