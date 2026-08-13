using System.Collections.Generic;
using UnityEngine;

public class BombsSpawner : Spawner<Bomb>
{
    [SerializeField] private Bomb _BombPrefab;

    private Spawner<Bomb> _spawner;
    private List<Cube> _activeCubes = new List<Cube>();

    private void Awake()
    {
        Init(_BombPrefab);
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
        Spawn(vector3);
    }
}
