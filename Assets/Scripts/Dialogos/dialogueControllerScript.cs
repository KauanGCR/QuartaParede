using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class dialogueControllerScript : MonoBehaviour
{
    [Header("Referência ao Movimento do Jogador")]
    private GameObject player;        // Referência ao GameObject do jogador
    private reiScript playerMovement; // Referência ao script de movimento do jogador
    void Start()
    {
        // Obtém o script de movimento do jogador
        player = GameObject.FindWithTag("Player");
        playerMovement = player.GetComponent<reiScript>();
    }

    void Update()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            playerMovement.freeze = true;
        }
        else
        {
            playerMovement.freeze = false;
        }
    }
}