using UnityEngine;
using UnityEngine.SceneManagement;

public class ZonaDeMorte : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.CompareTag("Player"))
    {
        
        collision.GetComponent<PlayerMovement>().VoltarParaCheckPoint();
    }
    }
}