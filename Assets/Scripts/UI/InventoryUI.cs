using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        inventory.OnChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        inventory.OnChanged -= Refresh;
    }

    private void Refresh()
    {
        label.text = $"Дерево: {inventory.Get(ResourceType.Wood)}    " +
                     $"Пластик: {inventory.Get(ResourceType.Plastic)}";
    }
}