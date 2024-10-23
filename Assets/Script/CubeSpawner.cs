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

    public float cubeSpeed = 3000f; // 큐브 이동 속도
    public float startDelay = 1f; // 음악 시작까지의 지연 시간
    public float stopspawnTime = 2f; // 큐브 스폰을 멈출 시간

    private float nextSpawnTime;
    private float spawnInterval;
    private int lastSpawnIndex = -1;
    private bool isPlaying = false;
    private float musicStartTime;

    void Start()
    {
        spawnInterval = 60f / BPM;
        musicStartTime = Time.time + startDelay;

        // 게임 시작 시퀀스 시작
        StartCoroutine(GameStartSequence());
    }

    IEnumerator GameStartSequence()
    {
        // 첫 번째 큐브들을 미리 스폰
        PreSpawnCubes();

        // 음악 시작 시간까지 대기
        yield return new WaitForSeconds(startDelay);

        // 음악 재생 시작
        musicSource.Play();
        isPlaying = true;
        nextSpawnTime = Time.time;
    }

    void PreSpawnCubes()
    {
        // 시작 지연 시간동안 생성될 큐브들을 미리 계산하여 스폰
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
                    // timeToReach 시간 동안 목표 지점까지 도달하도록 속도 계산
                    float distance = 3000f; // 목표 거리
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

        // 오디오의 남은 시간이 2초 이하일 때 스폰 중지
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
            // timeToReach 시간 동안 목표 지점까지 도달하도록 속도 계산
            float distance = 3000f; // 목표 거리
            float speed = distance / timeToReach;
            rb.velocity = note.transform.forward * speed;
        }
    }

    public void StopSpawning()
    {
        isPlaying = false;
    }
}
