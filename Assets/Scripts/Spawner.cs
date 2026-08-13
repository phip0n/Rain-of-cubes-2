using System;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private int _defaultCapacity;
    [SerializeField] private int _maxPoolSize;

    private T _prefab;
    private ObjectPool<T> _pool;
    private int _objectsCount = 0;

    public event Action<int> CountChanged;
    public event Action<int, int> ActiveCountChanged;

    protected void Init( T prefab)
    {
        _prefab = prefab;
        _pool = new ObjectPool<T>
            (
            createFunc: () => Create(),
            actionOnDestroy: (t) => Destroy(t),
            defaultCapacity: _defaultCapacity,
            maxSize: _maxPoolSize,
            collectionCheck: true
            );
    }

    protected T Spawn(Vector3 position)
    {
        T t = _pool.Get();
        t.SetPosition(position);
        t.Init();
        _objectsCount++;
        CountChanged?.Invoke(_objectsCount);
        ActiveCountChanged?.Invoke(_pool.CountAll, _pool.CountActive);
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
        ActiveCountChanged?.Invoke(_pool.CountAll, _pool.CountActive);
    }

    private void Destroy(T t)
    {
        t.Deactivacting -= Release;
        MonoBehaviour.Destroy(t.gameObject);
        ActiveCountChanged?.Invoke(_pool.CountAll, _pool.CountActive);
    }
}