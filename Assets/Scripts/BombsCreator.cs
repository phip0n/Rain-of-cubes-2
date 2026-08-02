using System.Collections.Generic;
using UnityEngine;

public class BombsCreator : MonoBehaviour
{
    [SerializeField] private Bomb _prefab;
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxPoolSize;
    [SerializeField] private Display _display;

    private Spawner<Bomb> _spawner;
    private List<Cube> _activeCubes = new List<Cube>();

    private void Awake()
    {
        _spawner = new Spawner<Bomb>(_prefab, _defaultCapacity, _maxPoolSize, out SpawnerInfo spawnerInfo);

        if (_display != null)
            _display.SetSpawnerInfo(spawnerInfo);
    }

    private void OnEnable()
    {
        if (_activeCubes.Count > 0)
        {
            foreach (Cube cube in _activeCubes)
            {
                cube.Exploding += CreateBomb;
                cube.Deactivacting += RemoveCube;
            }
        }
    }

    private void OnDisable()
    {
        foreach (Cube cube in _activeCubes)
        {
            cube.Exploding -= CreateBomb;
            cube.Deactivacting -= RemoveCube;
        }
    }

    public void AddCube(Cube cube)
    {
        _activeCubes.Add(cube);
        cube.Exploding += CreateBomb;
        cube.Deactivacting += RemoveCube;
    }

    private void RemoveCube(ISpawnable spawnable)
    {
        Cube cube = (Cube)spawnable;
        _activeCubes.Remove(cube);
        cube.Exploding -= CreateBomb;
        cube.Deactivacting -= RemoveCube;
    }

    private void CreateBomb(Vector3 vector3)
    {
        _spawner.Spawn(vector3);
    }
}
