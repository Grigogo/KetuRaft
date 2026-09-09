using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] debrisPrefabs;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDistance = 60f;
    [SerializeField] private float laneHalfWidth = 20f;
    [SerializeField] private float despawnDistance = 80f;
    [SerializeField] private int maxAlive = 40;

    private readonly List<GameObject> alive = new();
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && !OceanCurrent.Instance.IsCalm && alive.Count < maxAlive)
        {
            timer = spawnInterval * Random.Range(0.6f, 1.4f);
            Spawn();
        }

        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null) { alive.RemoveAt(i); continue; }

            if (alive[i].transform.position.magnitude > despawnDistance)
            {
                Destroy(alive[i]);
                alive.RemoveAt(i);
            }
        }
    }

    private void Spawn()
    {
        Vector3 dir = OceanCurrent.Instance.Velocity.normalized;
        Vector3 side = Vector3.Cross(Vector3.up, dir);

        Vector3 pos = -dir * spawnDistance
                      + side * Random.Range(-laneHalfWidth, laneHalfWidth);
        pos.y = 0.05f;

        GameObject prefab = debrisPrefabs[Random.Range(0, debrisPrefabs.Length)];
        alive.Add(Instantiate(prefab, pos, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)));
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || OceanCurrent.Instance == null) return;

        Vector3 dir = OceanCurrent.Instance.Velocity.normalized;
        Vector3 side = Vector3.Cross(Vector3.up, dir);
        Vector3 lineCenter = -dir * spawnDistance;

        // жёлтая линия — фронт спавна («выше по течению»)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(lineCenter - side * laneHalfWidth,
            lineCenter + side * laneHalfWidth);

        // красное кольцо — граница деспавна
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, despawnDistance);

        // зелёные стрелки — куда сейчас движется каждый обломок
        Gizmos.color = Color.green;
        foreach (var d in alive)
        {
            if (d == null) continue;
            Gizmos.DrawRay(d.transform.position, OceanCurrent.Instance.Velocity * 5f);
        }
    }
}