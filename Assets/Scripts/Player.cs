using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

#region VeryPersonalAndImportantDontTouchOrOpen
[HelpURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=RDdQw4w9WgXcQ")]
#endregion
public class Player : NetworkBehaviour
{
    private CharacterProperties _myCharacter;
    [SerializeField]
    private Renderer modelRenderer;

    [Networked, OnChangedRender(nameof(OnCharacterIdChanged))]
    public int CharacterID { get; set; }

    private CharacterProperties _character;

    private int _placeableAreaLayer;

    public override void Spawned()
    {
        base.Spawned();
        OnCharacterIdChanged();
        _placeableAreaLayer = LayerMask.NameToLayer("PlaceableArea");
    }

    public void SetCharacter(CharacterProperties character)
    {
        if (character == null) return;
        CharacterID = character.CharacterID;
        _character = character;
    }

    private void OnCharacterIdChanged()
    {        
        _myCharacter = CharacterProperties.GetByID(CharacterID);
        if (_myCharacter != null)
        {
            modelRenderer.material.color = _myCharacter.characterColor;
        }
    }

    void Update()
    {
        if (!Object.HasInputAuthority) return;
        if (Mouse.current.leftButton.wasPressedThisFrame && MatchManager.Instance != null)
        {            
            var screenPos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.gameObject.layer == _placeableAreaLayer)
                {
                    MatchManager.Instance.RequestPlacePlaceable(CharacterID, hit.point);
                }
                else 
                {
                    // ik this is not optimal but it might just be 3am
                    var placeable = hit.transform.GetComponentInParent<PlaceableObject>();
                    if (placeable != null)
                        MatchManager.Instance.RequestDeletePlaceable(placeable.Object.Id);
                }
            }
        }
    }
}
