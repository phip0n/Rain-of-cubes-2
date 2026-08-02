using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Bomb : MonoBehaviour, ISpawnable
{
    [SerializeField] private float _range = 2;
    [SerializeField] private float _force = 2;
    [SerializeField] private float _minExplosionTime = 2;
    [SerializeField] private float _maxExplosionTime = 5;

    private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();
    private Renderer _renderer;

    public event Action<ISpawnable> Deactivacting;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnDisable()
    {
        Deactivacting?.Invoke(this);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Init()
    {
        SetAlpha(1);
        gameObject.SetActive(true);
        StartCoroutine(StartExploding());
    }

    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddForce((hit.transform.position - transform.position).normalized * _force);
            }
        }

        gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color color = _renderer.material.color;
        _renderer.material.color = new Color(color.r, color.g, color.b, math.clamp(alpha, 0, 1));
    }

    private IEnumerator StartExploding()
    {
        float time = 0;
        float explosionTime = UnityEngine.Random.Range(_minExplosionTime, _maxExplosionTime);

        while (time !< explosionTime)
        {
            time += Time.deltaTime;
            SetAlpha(1 - time / explosionTime);
            yield return _waitForEndOfFrame;
        }

        Explode();
    }
}