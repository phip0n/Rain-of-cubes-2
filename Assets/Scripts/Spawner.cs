using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> where T : MonoBehaviour, ISpawnable
{
    private T _prefab;
    private ObjectPool<T> _pool;
    private int _objectsCount = 0;
    private SpawnerInfo _spawnerInfo;

    public Spawner( T prefab, int defaultCapacity, int maxSize, out SpawnerInfo spawnerInfo)
    {
        _prefab = prefab;
        _pool = new ObjectPool<T>
            (
            createFunc: () => Create(),
            actionOnDestroy: (t) => Destroy(t),
            defaultCapacity: defaultCapacity,
            maxSize: maxSize,
            collectionCheck: true
            );

        _spawnerInfo = new SpawnerInfo();
        spawnerInfo = _spawnerInfo;
    }

    public T Spawn(Vector3 position)
    {
        T t = _pool.Get();
        t.SetPosition(position);
        t.Init();
        _objectsCount++;
        _spawnerInfo.SetCount(_objectsCount);
        _spawnerInfo.SetActiveCount(_pool.CountAll, _pool.CountActive);
        Debug.Log(_objectsCount);
        return t;
    }
    private T Create()
    {
        T t = MonoBehaviour.Instantiate(_prefab, Vector3.zero, Quaternion.identity);
        t.Deactivacting += Release;
        return t;
    }

    private void Release(ISpawnable t)
    {
        _pool.Release((T)t);
        _spawnerInfo.SetActiveCount(_pool.CountAll, _pool.CountActive);
    }

    private void Destroy(T t)
    {
        t.Deactivacting -= Release;
        MonoBehaviour.Destroy(t.gameObject);
        _spawnerInfo.SetActiveCount(_pool.CountAll, _pool.CountActive);
    }
}