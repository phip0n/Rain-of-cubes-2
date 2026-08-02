using TMPro;
using UnityEngine;

public class Display : MonoBehaviour
{
    [SerializeField] private TMP_Text _creationsText;
    [SerializeField] private TMP_Text _poolText;
    [SerializeField] private TMP_Text _activeText;

    private SpawnerInfo _spawnerInfo;

    private void OnDisable()
    {
        _spawnerInfo.CountChanged -= ShowCreationsCount;
        _spawnerInfo.ActiveCountChanged -= ShowPoolInfo;
    }

    public void SetSpawnerInfo(SpawnerInfo spawnerInfo)
    {
        _spawnerInfo = spawnerInfo;
        _spawnerInfo.CountChanged += ShowCreationsCount;
        _spawnerInfo.ActiveCountChanged += ShowPoolInfo;
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