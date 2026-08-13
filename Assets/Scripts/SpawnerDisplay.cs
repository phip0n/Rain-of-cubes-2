using TMPro;
using UnityEngine;

public class SpawnerDisplay<T> : MonoBehaviour where T : MonoBehaviour, ISpawnable
{
    [SerializeField] private Spawner<T> _spawner;
    [SerializeField] private TMP_Text _creationsText;
    [SerializeField] private TMP_Text _poolText;
    [SerializeField] private TMP_Text _activeText;

    private void OnEnable()
    {
        _spawner.CountChanged += ShowCreationsCount;
        _spawner.ActiveCountChanged += ShowPoolInfo;
    }

    private void OnDisable()
    {
        _spawner.CountChanged -= ShowCreationsCount;
        _spawner.ActiveCountChanged -= ShowPoolInfo;
    }

    public void ShowCreationsCount(int count)
    {
        _creationsText.text = count.ToString();
    }

    public void ShowPoolInfo(int allObjects, int activeObjects)
    {
        _poolText.text = allObjects.ToString();
        _activeText.text = activeObjects.ToString();
    }
}