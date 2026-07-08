using Fusion;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    [Networked]
    private TickTimer lifeTimer { get; set; }
    
    [SerializeField] private float speed = 10f;

    public override void Spawned()
    {
        if (!Object.HasStateAuthority) return;
        lifeTimer = TickTimer.CreateFromSeconds(Runner, 10f);
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;
        transform.position += transform.forward * speed * Time.fixedDeltaTime;
        
        if (lifeTimer.Expired(Runner))
        {   
            Runner.Despawn(Object);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Object.HasStateAuthority) return;
        var hitObject = other.GetComponentInParent<NetworkObject>();
        if (!hitObject) return;

        if (hitObject.TryGetComponent(out Player player) && hitObject.InputAuthority == Object.InputAuthority)
            return;

        Debug.Log("Projectile hit " + hitObject.name);
        Runner.Despawn(Object);
    }

    
}
