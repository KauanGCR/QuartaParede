using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor; // Certifique-se de que o Dialogue Editor esteja sendo usado

public class dialogueControllerScript : MonoBehaviour
{
    [Header("Referência ao Movimento do Jogador")]
    public GameObject player;        // Referência ao GameObject do jogador
    public reiScript playerMovement; // Referência ao script de movimento do jogador

    void Start()
    {
        // Obtém o script de movimento do jogador
        playerMovement = player.GetComponent<reiScript>();

        // Inscreve-se nos eventos do Dialogue Editor
        ConversationManager.OnConversationStarted += OnDialogoIniciado;
        ConversationManager.OnConversationEnded += OnDialogoFinalizado;
    }

    void OnDestroy()
    {
        // Remove as inscrições nos eventos para evitar erros
        ConversationManager.OnConversationStarted -= OnDialogoIniciado;
        ConversationManager.OnConversationEnded -= OnDialogoFinalizado;
    }

    void OnDialogoIniciado()
    {
        // Desativa o script de movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
    }

    void OnDialogoFinalizado()
    {
        // Reativa o script de movimento do jogador
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
    }
}