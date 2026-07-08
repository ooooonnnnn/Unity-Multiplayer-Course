using System;
using Fusion;
using UnityEngine;

public class HittableTarget : NetworkBehaviour
{
    [SerializeField] private Renderer modelRenderer;

    [Networked, OnChangedRender(nameof(OnHitStateChanged))]
    private bool isHit {get; set;}

    public override void Spawned()
    {
        OnHitStateChanged();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        if (isHit) return;

        var hitObject = other.GetComponentInParent<NetworkObject>();
        if (!hitObject) return;
        if (!other.GetComponentInParent<Projectile>()) return;

        Debug.Log(gameObject.name + " was hit by " + hitObject.name);
        isHit = true;
    }

    private void OnHitStateChanged()
    {
        modelRenderer.material.color = isHit ? Color.red : Color.blue;
    }
}
