using Fusion;
using UnityEngine;

public class PlacementManager : NetworkBehaviour
{
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void PlacePlaceableRPC(int characterID, Vector3 position, RpcInfo info = default)
    {
        var props = CharacterProperties.GetByID(characterID);
        if (props == null) return;

        if (!props.spawnObject.TryGetComponent(out PlaceableObject placeable)) return;

        Runner.Spawn(props.spawnObject, position + placeable.GetGPOffset(), null, info.Source);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DeletePlaceableRPC(NetworkId targetId, RpcInfo info = default)
    {
        if (!Runner.TryFindObject(targetId, out NetworkObject networkObject)) return;
        if (networkObject.GetComponentInChildren<PlaceableObject>() == null) return;

        if (networkObject.StateAuthority == info.Source || Object.HasStateAuthority)
        {
            Runner.Despawn(networkObject);
        }
    }
}