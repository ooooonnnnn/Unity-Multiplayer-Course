using Fusion;
using UnityEngine;

public class NetworkObjectActiveState : NetworkBehaviour
{
    [SerializeField] private GameObject target;
    
    [Networked, OnChangedRender(nameof(NetworkUpdateActive))]
    public byte IsActive { get; set; } = 1;
    
    public override void Spawned()
    {
        base.Spawned();
        NetworkUpdateActive();
    }
    
    private void NetworkUpdateActive()
    {
        target.SetActive(IsActive == 1);
    }
    
    public void SetActive(bool active)
    {
        print(HasStateAuthority);
        IsActive = (byte)(active ? 1 : 0);
    }
}
