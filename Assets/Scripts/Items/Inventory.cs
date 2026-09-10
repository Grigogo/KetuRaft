using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly Dictionary<ResourceType, int> counts = new();

    public event Action OnChanged;

    public void Add(ResourceType type, int amount)
    {
        counts.TryGetValue(type, out int current);
        counts[type] = current + amount;
        OnChanged?.Invoke();
    }

    public int Get(ResourceType type) =>
        counts.TryGetValue(type, out int value) ? value : 0;
}