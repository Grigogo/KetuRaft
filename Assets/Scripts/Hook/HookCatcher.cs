using UnityEngine;

public class HookCatcher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponentInParent<DebrisItem>();
        if (item == null) return;

        var drift = item.GetComponent<FloatingDebris>();
        if (drift != null) drift.enabled = false;

        item.transform.SetParent(transform);
    }
}