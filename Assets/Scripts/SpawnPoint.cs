using System.Threading.Tasks;
using Fusion;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Vector2 minRandomOffset;

    [SerializeField]
    private Vector2 maxRandomOffset;

    [SerializeField]
    private GameObject _playerPrefab;

    public async Task SpawnGivenPlayer(PlayerRef player, CharacterProperties character, bool useRandomOffset = true)
    {
        if (!SinglePeer_NetworkRunnerManager.Instance.NetworkRunner.IsSharedModeMasterClient) return;

        var offset = Vector3.up;

        if (useRandomOffset)
        {
            offset += new Vector3(
                Random.Range(minRandomOffset.x, maxRandomOffset.x),
                0,
                Random.Range(minRandomOffset.y, maxRandomOffset.y)
            );
        }
        
        var spawned = await SinglePeer_NetworkRunnerManager.Instance.NetworkRunner.SpawnAsync(_playerPrefab, transform.position + offset, null, player);
        spawned.GetComponent<Player>().SetCharacter(character);
    }
}
