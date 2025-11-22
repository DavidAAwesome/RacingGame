using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform[] waypoints;

    public int Count => waypoints.Length;

    public Transform GetWaypoint(int index)
    {
        if (Count == 0) return null;
        return waypoints[index % Count];
    }

    private void OnDrawGizmos()
    {
        // Draw path in Scene view
        Gizmos.color = Color.yellow;
        if (waypoints == null || waypoints.Length < 2) return;

        for (int i = 0; i < waypoints.Length; i++)
        {
            var a = waypoints[i];
            var b = waypoints[(i + 1) % waypoints.Length];
            if (a && b)
            {
                Gizmos.DrawSphere(a.position, 0.3f);
                Gizmos.DrawLine(a.position, b.position);
            }
        }
    }
}