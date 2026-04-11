using UnityEngine;
using UnityEngine.UI; // Necessário para mexer em imagens

public class GerenciadorVidaUI : MonoBehaviour
{
    // Arraste as 3 imagens dos corações para cá no Inspector
    public Image[] coracoes; 
    
    // Arraste a imagem do coração "vazio" (cinza ou transparente)
    public Sprite coracaoVazio;
    public Sprite coracaoCheio;

    public void AtualizarCoracoes(int vidaAtual)
    {
        for (int i = 0; i < coracoes.Length; i++)
        {
            // Se o índice for menor que a vida atual, mostra cheio, senão mostra vazio
            if (i < vidaAtual)
            {
                coracoes[i].sprite = coracaoCheio;
                coracoes[i].enabled = true; // Ou você pode apenas esconder: coracoes[i].enabled = true;
            }
            else
            {
                coracoes[i].sprite = coracaoVazio;
                // Se não tiver imagem de coração vazio, pode apenas desativar a imagem:
                // coracoes[i].enabled = false; 
            }
        }
    }
}