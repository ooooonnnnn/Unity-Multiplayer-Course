using System;
using Fusion;
using UnityEngine;

public class HittableTarget : NetworkBehaviour
{
    [SerializeField] private Renderer modelRenderer;
    [SerializeField] private ParticleSystem hitEffectPrefab;
    [SerializeField] private float flashDuration = 1f;

    [Networked, OnChangedRender(nameof(OnHitStateChanged))]
    private bool isHit {get; set;}
    
    private Color _originalColor;
    private readonly Color _hitColor = Color.red;
    

    public override void Spawned()
    {
        _originalColor = modelRenderer.material.color;
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

        RpcPlayHitEffect();
    }
    
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcPlayHitEffect()
    {
        ParticleSystem effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        effect.Play();
        Destroy(effect.gameObject, effect.main.duration);
    }

    private void OnHitStateChanged()
    {
        modelRenderer.material.color = isHit ? _hitColor : _originalColor;
    }
}
