using UnityEngine;
using UnityEngine.Tilemaps; 

public class CheckpointTile : MonoBehaviour
{
    [Header("Configurações do Tilemap")]
    public Tilemap mapaDoCheckpoint; // Arraste o Tilemap onde está o computador
    public TileBase tileSemVisto;    // O Tile do computador vazio
    public TileBase tileComVisto;    // O Tile do computador com o "visto"

    private bool jaSalvou = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !jaSalvou)
        {
            
            collision.GetComponent<PlayerMovement>().SalvarCheckpoint(transform.position);

            
            Vector3Int posicaoDoTile = mapaDoCheckpoint.WorldToCell(transform.position);
            
            if (mapaDoCheckpoint != null && tileComVisto != null)
            {
                mapaDoCheckpoint.SetTile(posicaoDoTile, tileComVisto);
            }

            jaSalvou = true;
            Debug.Log("Checkpoint: Tile trocado para o computador com visto!");
        }
    }
}