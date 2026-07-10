using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

#region VeryPersonalAndImportantDontTouchOrOpen
[HelpURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ&list=RDdQw4w9WgXcQ")]
#endregion
public class Player : NetworkBehaviour, IHitable
{
    private CharacterProperties _myCharacter;
    [SerializeField]
    private Renderer modelRenderer;
    [SerializeField]
    private Canvas playerUI;
    [SerializeField]
    private TMP_Text HPLabel;

    [Networked, OnChangedRender(nameof(OnCharacterIdChanged))]
    public int CharacterID { get; set; }

    private CharacterProperties _character;
    private int _placeableAreaLayer;

    [Networked, OnChangedRender(nameof(OnHPChanged))]
    private float _hp { get; set; }

    [SerializeField]
    private float startingHP;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private CharacterController _controller;


    public override void Spawned()
    {
        base.Spawned();
        OnCharacterIdChanged();
        _placeableAreaLayer = LayerMask.NameToLayer("PlaceableArea");

        _hp = startingHP;

        if (playerUI) playerUI.gameObject.SetActive(Object.HasStateAuthority);
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

    private void OnHPChanged()
    {
        HPLabel.text = $"Health: {_hp:F2}";
    }

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasInputAuthority || !_controller) return;

        var moveDirection = Vector3.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
                moveDirection.z += 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
                moveDirection.z -= 1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                moveDirection.x -= 1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                moveDirection.x += 1;
        }

        _controller.Move(moveDirection.normalized * moveSpeed * Runner.DeltaTime);
    }

    void Update()
    {
        if (!Object.HasInputAuthority) return;
        
        if (Mouse.current.leftButton.wasPressedThisFrame && MatchManager.Instance)
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
                    {
                        var netObj = placeable.GetComponent<NetworkObject>();
                        if (netObj != null)
                            MatchManager.Instance.RequestDeletePlaceable(netObj.Id);
                    }
                }
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame && MatchManager.Instance)
        {
            var screenPos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 origin = transform.position;
                Vector3 direction = hit.point - origin;
                direction.y = 0f;

                if (direction.sqrMagnitude > 0.0001f)
                {
                    MatchManager.Instance.RequestSpawnProjectile(CharacterID, origin, direction.normalized);
                }
            }
        }
    }

    public void OnHit(DamageData data)
    {
        _hp -= data.damage;
    }
}