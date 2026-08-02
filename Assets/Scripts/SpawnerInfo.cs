using System;

public class SpawnerInfo
{
    public event Action<int> CountChanged;
    public event Action<int, int> ActiveCountChanged;

    public void SetCount(int count)
    {
        CountChanged?.Invoke(count);
    }

    public void SetActiveCount(int allCount, int activeCount)
    {
        ActiveCountChanged?.Invoke(allCount, activeCount);
    }
}
