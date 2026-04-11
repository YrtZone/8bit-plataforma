using UnityEngine;
using UnityEngine.Tilemaps; // Necessário para Tilemap
using System.Collections;
using UnityEngine.SceneManagement;

public class FogoPiscante : MonoBehaviour
{
    public float duracaoTransicao = 0.5f; 
    public float tempoAceso = 2.0f;
    public float tempoApagado = 2.0f;

    private TilemapRenderer tilemapRenderer; 
    private Tilemap tilemap;
    private Color corOriginal;
    private BoxCollider2D colisor;
    private bool estaAtivo;

    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        colisor = GetComponent<BoxCollider2D>();

        if (tilemap != null)
        {
            corOriginal = tilemap.color;
            StartCoroutine(CicloFogo());
        }
    }

    IEnumerator CicloFogo()
    {
        while (true)
        {
            estaAtivo = true;
            if (colisor != null) colisor.enabled = true;
            yield return StartCoroutine(MudarTransparencia(0f, 1f)); // Aparece
            yield return new WaitForSeconds(tempoAceso);

            yield return StartCoroutine(MudarTransparencia(1f, 0f)); // Some
            estaAtivo = false;
            if (colisor != null) colisor.enabled = false;
            yield return new WaitForSeconds(tempoApagado);
        }
    }

    IEnumerator MudarTransparencia(float inicio, float fim)
    {
        float t = 0;
        while (t < duracaoTransicao)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(inicio, fim, t / duracaoTransicao);
            tilemap.color = new Color(corOriginal.r, corOriginal.g, corOriginal.b, alpha);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
{
    if (estaAtivo && collision.CompareTag("Player"))
    {
        // Puxa o script de movimento (que agora tem a vida junto)
        PlayerMovement scriptPlayer = collision.GetComponent<PlayerMovement>();

        if (scriptPlayer != null)
        {
            scriptPlayer.TomarDano(1);
        }
    }
}
}