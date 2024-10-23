using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject notes;
    public Transform[] pos;
    public AudioSource musicSource;
    public float detectionThreshold = 0.1f;
    public int BPM;

    [Range(0f, 1f)]
    public float spawnProbability = 0.7f;

    public float cubeSpeed = 3000f; // ť�� �̵� �ӵ�
    public float startDelay = 1f; // ���� ���۱����� ���� �ð�
    public float stopspawnTime = 2f; // ť�� ������ ���� �ð�

    private float nextSpawnTime;
    private float spawnInterval;
    private int lastSpawnIndex = -1;
    private bool isPlaying = false;
    private float musicStartTime;

    void Start()
    {
        spawnInterval = 60f / BPM;
        musicStartTime = Time.time + startDelay;

        // ���� ���� ������ ����
        StartCoroutine(GameStartSequence());
    }

    IEnumerator GameStartSequence()
    {
        // ù ��° ť����� �̸� ����
        PreSpawnCubes();

        // ���� ���� �ð����� ���
        yield return new WaitForSeconds(startDelay);

        // ���� ��� ����
        musicSource.Play();
        isPlaying = true;
        nextSpawnTime = Time.time;
    }

    void PreSpawnCubes()
    {
        // ���� ���� �ð����� ������ ť����� �̸� ����Ͽ� ����
        float currentTime = 0f;
        while (currentTime < startDelay)
        {
            if (Random.value <= spawnProbability)
            {
                GameObject note = Instantiate(notes, pos[1]);

                CubeNote cubeNote = note.GetComponent<CubeNote>();
                if (cubeNote != null)
                {
                    cubeNote.spawnDirection = note.transform.forward;
                }

                Rigidbody rb = note.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.drag = 0f;
                    // timeToReach �ð� ���� ��ǥ �������� �����ϵ��� �ӵ� ���
                    float distance = 3000f; // ��ǥ �Ÿ�
                    float speed = distance / (startDelay - currentTime);
                    rb.velocity = note.transform.forward * speed;
                }
            }
            currentTime += spawnInterval;
        }
    }

    void Update()
    {
        if (!isPlaying) return;

        // ������� ���� �ð��� 2�� ������ �� ���� ����
        if (musicSource.clip.length - musicSource.time <= stopspawnTime)
        {
            StopSpawning();
        }

        if (Time.time >= nextSpawnTime)
        {
            if (Random.value <= spawnProbability)
            {
                SpawnCube(startDelay);
            }
            nextSpawnTime = Time.time + spawnInterval - 0.05f;
        }        
    }

    void SpawnCube(float timeToReach)
    {
        int i;
        do
        {
            i = Random.Range(0, pos.Length);
        } while (i == lastSpawnIndex);

        lastSpawnIndex = i;

        GameObject note = Instantiate(notes, pos[i]);
        note.transform.localPosition = Vector3.zero;

        int rotationIndex = Random.Range(0, 4);
        note.transform.Rotate(transform.forward, 90 * rotationIndex);

        CubeNote cubeNote = note.GetComponent<CubeNote>();
        if (cubeNote != null)
        {
            cubeNote.spawnDirection = note.transform.forward;
        }

        Rigidbody rb = note.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.drag = 0f;
            // timeToReach �ð� ���� ��ǥ �������� �����ϵ��� �ӵ� ���
            float distance = 3000f; // ��ǥ �Ÿ�
            float speed = distance / timeToReach;
            rb.velocity = note.transform.forward * speed;
        }
    }

    public void StopSpawning()
    {
        isPlaying = false;
    }
}
