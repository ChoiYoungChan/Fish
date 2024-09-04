using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonClass<PoolManager>
{
    [SerializeField] private BoxCollider _collider;

    public Vector3 GetRandomTargetPoint()
    {
        Vector3 center = _collider.center;
        Vector3 size = _collider.size;

        // Generate random points in the collider's local coordinate system
        Vector3 randomPoint = new Vector3(
            Random.Range((center.x - size.x / 2), (center.x + size.x / 2)),
            Random.Range( -(size.y / 2), (center.y + size.y / 2)),
            Random.Range((center.z - size.z / 2), (center.z + size.z / 2))
        );

        // Conversion from local coordinate system to world coordinate system
        return _collider.transform.TransformPoint(randomPoint);
    }
}
