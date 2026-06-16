using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementManager : NetworkBehaviour
{
    public override void Spawned()
    {
        base.Spawned();
        Debug.Log($"PlacementManager Spawned. HasStateAuthority: {Object.HasStateAuthority}. IsNetworkObjectValid: {Object != null}");
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void PlacePlaceableRPC(int characterID, Vector3 position, RpcInfo info = default)
    {
        if (!Object.HasStateAuthority) return;

        var props = CharacterProperties.GetByID(characterID);

        if (props == null) return;

        if (!props.spawnObject.TryGetComponent(out PlaceableObject placeable)) return;
        
        Runner.Spawn(props.spawnObject, position + placeable.GetGPOffset(), null);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DeletePlaceableRPC(NetworkId targetId)
    {
        if (Runner.TryFindObject(targetId, out NetworkObject networkObject) && networkObject.GetComponentInParent<PlaceableObject>())
        {
            Runner.Despawn(networkObject);
        }
    }
}
