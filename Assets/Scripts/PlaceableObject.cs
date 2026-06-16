using Fusion;
using UnityEngine;

public class PlaceableObject : NetworkBehaviour
{
    [SerializeField]
    private Transform groundPoint;

    public Vector3 GetGPOffset()
    {
        return -groundPoint.localPosition;
    }
}
