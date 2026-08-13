using System;
using System.Collections;
using UnityEngine;

public class CubesSpawner : Spawner<Cube>
{
    [SerializeField] private BombsSpawner _bombsSpawner;
    [SerializeField] private int _cubesPerSpawn;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _spawnTime;
    [SerializeField] private Vector3 _spawnSize;

    private void Awake()
    {
        Init(_cubePrefab);
    }

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        WaitForSeconds time = new WaitForSeconds(_spawnTime);

        while (enabled)
        {
            yield return time;

            for (int i = 0; i < _cubesPerSpawn; i++)
            {
                Cube cube;
                Vector3 position = new Vector3(UnityEngine.Random.Range(-_spawnSize.x, _spawnSize.x),
                    UnityEngine.Random.Range(-_spawnSize.y, _spawnSize.y),
                    UnityEngine.Random.Range(-_spawnSize.z, _spawnSize.z));
                position += transform.position;
                cube = Spawn(position);
                _bombsSpawner.AddCube(cube);
            }
        }
    }
}