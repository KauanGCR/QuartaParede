using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;
using Cinemachine;

public class CenaScriptada : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera; // Referência à câmera do Cinemachine
    public Transform rei; // Referência ao rei (para voltar no final)
    public Transform npc1; // NPC 1
    public Transform npc2; // NPC 2
    public Vector3 pontoFinalNpc1 = new Vector3(-116.5f, -1.75f, 0f); // Ponto final do NPC 1
    public Vector3 pontoFinalNpc2 = new Vector3(-116.5f, -1.75f, 0f); // Ponto final do NPC 2
    public Vector3 pontoAtivacao = new Vector3(-81f, -1.75f, 0f); // Ponto de ativação da cena
    public NPCConversation dialogo; // Trigger do Dialogue Editor

    private bool cenaIniciada = false; // Garante que a cena ocorre apenas uma vez
    private bool dialogoTerminado = false; // Indica se o diálogo terminou

    private npcScript scriptNpc1; // Script do NPC 1
    private npcScript scriptNpc2; // Script do NPC 2

    private void Start()
    {
        // Obtém os scripts dos NPCs
        scriptNpc1 = npc1.GetComponent<npcScript>();
        scriptNpc2 = npc2.GetComponent<npcScript>();

        if (scriptNpc1 == null || scriptNpc2 == null)
        {
            Debug.LogError("Os scripts dos NPCs não foram encontrados!");
        }

        if (cinemachineCamera == null)
        {
            Debug.LogError("Câmera do Cinemachine não atribuída!");
        }
    }

    private void Update()
{
    if (!cenaIniciada && Vector3.Distance(rei.position, pontoAtivacao) < 1f)
    {
        IniciarCenaScriptada();
    }

    if (dialogoTerminado)
    {
        // Chamando o método de movimento
        bool npc1Movido = scriptNpc1 != null && scriptNpc1.MoverParaPonto(pontoFinalNpc1);
        bool npc2Movido = scriptNpc2 != null && scriptNpc2.MoverParaPonto(pontoFinalNpc2);

        if (npc1Movido)
        {
            npc1.gameObject.SetActive(false); // Esconde o NPC 1
        }

        if (npc2Movido)
        {
            npc2.gameObject.SetActive(false); // Esconde o NPC 2
            EncerrarCenaScriptada();
        }
    }
}

    private void IniciarCenaScriptada()
    {
        cenaIniciada = true;

        // Fazer a câmera do Cinemachine seguir o NPC 1
        AlterarAlvoCamera(npc2);

        // Iniciar o diálogo
        if (ConversationManager.Instance != null && dialogo != null)
        {
            ConversationManager.Instance.StartConversation(dialogo);
        }
        else
        {
            Debug.LogWarning("Dialogue Manager ou diálogo não configurado corretamente.");
        }
    }

    public void FinalizarDialogo()
    {
        // Chamado pelo Dialogue Editor quando o diálogo terminar
        dialogoTerminado = true;

        // Alterar a câmera para seguir o NPC 1 enquanto ele se move
        AlterarAlvoCamera(npc2);
    }

    private void EncerrarCenaScriptada()
    {
        // Fazer a câmera voltar para o rei
        AlterarAlvoCamera(rei);

        // Desativar este script para evitar loops
        this.enabled = false;
    }

    private void AlterarAlvoCamera(Transform alvo)
    {
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = alvo;
            cinemachineCamera.LookAt = alvo;
        }
    }
}
