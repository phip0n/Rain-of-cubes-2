using System;
using System.Collections;
using UnityEngine;

public class CubesCreator : MonoBehaviour
{
    [SerializeField] private BombsCreator _bombsCreator;
    [SerializeField] private int _cubesPerSpawn;
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _spawnTime;
    [SerializeField] private Vector3 _spawnSize;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private Display _display;

    private Spawner<Cube> _spawner;

    public event Action<int> CountChanged;
    public event Action<int, int> ActiveCountChanged;

    private void Awake()
    {
        _spawner = new Spawner<Cube>(_prefab, _defaultCapacity, _maxPoolSize, out SpawnerInfo spawnerInfo);

        if (_display != null)
            _display.SetSpawnerInfo(spawnerInfo);
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
                cube = _spawner.Spawn(position);
                _bombsCreator.AddCube(cube);
            }
        }
    }
}