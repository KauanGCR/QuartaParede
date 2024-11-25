using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class mariaScript : MonoBehaviour
{
    public NPCConversation dialogo; // Referência para o diálogo do NPC
    public Collider2D mariaCollider; // Collider do NPC do diálogo
    private bool playerInRange = false; // Verifica se o jogador está no alcance
    private bool dialogoFinalizado = false; // Garante que o diálogo só seja mostrado uma vez
    private reiScript playerController; // Referência ao controlador do jogador

    private void Start()
    {
        // Busca o PlayerController na cena (ou associe no Inspector)
        playerController = FindObjectOfType<reiScript>();
    }

    private void Update()
    {
        // Só interage se o jogador estiver no alcance, possuindo um NPC, e ainda não tiver mostrado o diálogo
        if (playerInRange && !dialogoFinalizado && playerController.possuindo && Input.GetKeyDown(KeyCode.Q))
        {
            MostrarDialogo();
        }
    }

    void MostrarDialogo()
    {
        // Inicia o diálogo utilizando o gerenciador de conversas
        ConversationManager.Instance.StartConversation(dialogo);
        dialogoFinalizado = true; // Marca como já mostrado
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Se o Player ou NPC colidiu, ignora a colisão
        if (collision.CompareTag("Player") || collision.CompareTag("NPC"))
        {
            Physics2D.IgnoreCollision(collision, mariaCollider, true);

            // Apenas habilita a interação se for o Player
            if (collision.CompareTag("NPC"))
            {
                playerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Quando o Player ou NPC sai da área, restaura a colisão
        if (collision.CompareTag("Player") || collision.CompareTag("NPC"))
        {
            Physics2D.IgnoreCollision(collision, mariaCollider, false);

            // Se for o Player, desabilita a interação
            if (collision.CompareTag("NPC"))
            {
                playerInRange = false;
            }
        }
    }
}