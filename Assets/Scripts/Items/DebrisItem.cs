using UnityEngine;

public enum ResourceType { Wood, Plastic }

public class DebrisItem : MonoBehaviour
{
    [SerializeField] private ResourceType type = ResourceType.Wood;
    [SerializeField] private int amount = 1;

    public ResourceType Type => type;
    public int Amount => amount;
}
