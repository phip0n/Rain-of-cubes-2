using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour, ISpawnable
{
    [SerializeField] private float _minDyingTime = 2;
    [SerializeField] private float _maxDyingTime = 5;

    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Coroutine _waitForDeath;
    private bool _isDying = false;

    public event Action<ISpawnable> Deactivacting;
    public event Action<Vector3> Exploding;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDying == false && collision.gameObject.TryGetComponent<Platform>(out Platform platform))
        {
            Die();
        }
    }

    private void OnDisable()
    {
        Deactivacting?.Invoke(this);
    }

    public void Init()
    {
        if (_waitForDeath != null)
        {
            StopCoroutine(_waitForDeath);
        }

        transform.rotation = Quaternion.Euler(Vector3.zero);
        _renderer.material.color = Color.white;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _isDying = false;
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Die()
    {
        _isDying = true;
        float dyingTime = UnityEngine.Random.Range(_minDyingTime, _maxDyingTime);
        _renderer.material.color = UnityEngine.Random.ColorHSV();
        _waitForDeath = StartCoroutine(WaitForDeath(dyingTime));
    }

    private IEnumerator WaitForDeath(float time)
    {
        WaitForSeconds dyingTime = new WaitForSeconds(time);
        yield return dyingTime;
        Exploding?.Invoke(transform.position);
        gameObject.SetActive(false);
    }
}