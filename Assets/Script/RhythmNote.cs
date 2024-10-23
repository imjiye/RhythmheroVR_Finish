using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class RhythmNote : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject[] notes;
    public Transform[] pos; // ��带 ������ ��ġ��
    public float detectionThreshold = 0.1f; // ��Ʈ ������ ���� �Ӱ谪
    public float spawnInterval = 0.5f; // ��� ���� ����
    private float nextSpawnTime;

    void Update()
    {
        float[] spectrum = new float[256];
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // ��Ʈ ����
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > detectionThreshold && Time.time >= nextSpawnTime)
            {
                SpawnNote();
                nextSpawnTime = Time.time + spawnInterval;
                break; // ù ��° ��Ʈ ���� �� ���� ����
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

        // ť���� ������ ��ũ��Ʈ�� ����
        CubeNote cubeNote = note.GetComponent<CubeNote>();
        if (cubeNote != null)
        {
            cubeNote.spawnDirection = note.transform.forward; // ť���� ���� ���� ����
        }
    }
}
