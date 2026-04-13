using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 5f;
    public float climbSpeed = 4f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator anim;
    private GameObject visualDoPlayer;

    private float moveX;
    private float moveY;
    private bool estaNaEscada;
    private float gravidadePadrao;

    [Header("Sistema de Vida")]
    public int vidaMaxima = 3;
    private int vidaAtual;

    // Variáveis para o controle Touch
    private float inputTouchH;
    private float inputTouchV;

    // Funções que os botões da UI vão chamar
    public void SetInputHorizontal(float valor) { inputTouchH = valor; }
    public void SetInputVertical(float valor) { inputTouchV = valor; }
    
    public GerenciadorVidaUI scriptUI;
    private Vector3 pontoDeResgate;

    void Start()
    {
        vidaAtual = vidaMaxima;
        pontoDeResgate = transform.position;
        if(scriptUI != null) scriptUI.AtualizarCoracoes(vidaAtual);

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        visualDoPlayer = transform.childCount > 0 ? transform.GetChild(0).gameObject : gameObject;

        gravidadePadrao = rb.gravityScale;
        rb.freezeRotation = true;

        pontoDeResgate = transform.position;
    }

    void Update()
    { 
        // 1. Inputs de Movimento (Unificando Teclado + Touch)
        float tecladoH = 0;
        float tecladoV = 0;

        // Leitura do Teclado (New Input System)
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) tecladoH = 1;
        else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) tecladoH = -1;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) tecladoV = 1;
        else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) tecladoV = -1;

        // Soma as forças (Teclado + Touch) e limita entre -1 e 1
        moveX = Mathf.Clamp(tecladoH + inputTouchH, -1, 1);
        moveY = Mathf.Clamp(tecladoV + inputTouchV, -1, 1);

        // 2. Pulo (Teclado chamando a função pública)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Pular();
        }
        
        AtualizarVisual();
    }

    void FixedUpdate()
    {
        if (estaNaEscada)
        {
            // Na escada, tiramos a gravidade 
            rb.gravityScale = 0;
            rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * climbSpeed);
        }
        else
        {
            // Fora da escada, gravidade normal
            rb.gravityScale = gravidadePadrao;
            rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);
        }
    }

    void AtualizarVisual()
    {
        // Se estiver andando no chão ou subindo escada,
        bool movendo = Mathf.Abs(moveX) > 0.1f || (estaNaEscada && Mathf.Abs(moveY) > 0.1f);
        
        if (anim != null)
        {
            anim.SetBool("estaAndando", movendo);
            anim.speed = movendo ? 1f : 0f;
        }

        if (moveX != 0)
        {
            // Ajuste de escala para o player virar para o lado certo
            visualDoPlayer.transform.localScale = new Vector3(moveX > 0 ? 1 : -1, 1, 1);
        }
    }

    // 4. Detecção da Escada
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Escada") || collision.gameObject.layer == LayerMask.NameToLayer("Escada"))
        {
            estaNaEscada = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Escada") || collision.gameObject.layer == LayerMask.NameToLayer("Escada"))
        {
            estaNaEscada = false;
        }
    }

    public void TomarDano(int quantidade)
    {
        vidaAtual -= quantidade;
        Debug.Log("Vida do Player: " + vidaAtual);
        if(scriptUI != null) scriptUI.AtualizarCoracoes(vidaAtual);

        if (vidaAtual <= 0)
        {
            // Se a vida acabar, reinicia a fase
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            // Se ainda tem vida, volta apenas para o Checkpoint
            VoltarParaCheckPoint();
        }
    }

    public void SalvarCheckpoint(Vector3 novaPosicao)
    {
        pontoDeResgate = novaPosicao;
    }

    public void VoltarParaCheckPoint()
    {
        transform.position = pontoDeResgate;
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    // Função pública para o botão de Touch e Teclado chamarem
    public void Pular()
    {
        // Verificação para garantir que só pule no chão e fora da escada
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f && !estaNaEscada)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("Pulo executado!");
        }
    }
}