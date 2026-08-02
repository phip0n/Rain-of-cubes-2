using System;
using UnityEngine;

public interface ISpawnable
{
    public event Action<ISpawnable> Deactivacting;

    public void Init();
    public void SetPosition(Vector3 position);
}