using UnityEngine;

public class Controle : MonoBehaviour
{
    public PlayerMovement playerScript; // Arraste o Player para cá no Inspector

    // --- MOVIMENTAÇÃO HORIZONTAL ---
    public void ApertouDireita() { playerScript.SetInputHorizontal(1f); }
    public void ApertouEsquerda() { playerScript.SetInputHorizontal(-1f); }
    public void SoltouHorizontal() { playerScript.SetInputHorizontal(0f); }

    // --- MOVIMENTAÇÃO VERTICAL ---
    public void ApertouCima() { playerScript.SetInputVertical(1f); }
    public void ApertouBaixo() { playerScript.SetInputVertical(-1f); }
    public void SoltouVertical() { playerScript.SetInputVertical(0f); }

    // --- PULO (ADICIONADO) ---
    public void BotaoPular() 
    {
        if (playerScript != null)
    { 
        playerScript.Pular(); 
    }

    }
    
}