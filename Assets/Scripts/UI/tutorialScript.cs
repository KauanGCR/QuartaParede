using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class TutorialScript : MonoBehaviour
{
    [Header("Teclas")]
    public SpriteRenderer spriteA;
    public Sprite spriteAIdle;
    public Sprite spriteAPressed;

    public SpriteRenderer spriteD;
    public Sprite spriteDIdle;
    public Sprite spriteDPressed;

    public SpriteRenderer spriteE;
    public Sprite spriteEIdle;
    public Sprite spriteEPressed;

    public SpriteRenderer spriteEspaco;
    public Sprite spriteEspacoIdle;
    public Sprite spriteEspacoPressed;
    
    [Header("Elementos")]
    public NPCConversation dialogo;
    public GameObject botaoA;
    public GameObject botaoD;
    public GameObject fantasmaPopup;
    public GameObject possessaoPopup;
    public GameObject espaco;
    public GameObject botaoE;

    [Header("Player")]
    public Transform player;
    public Animator animator;

    [Header("Posições de Ativação")]
    public Vector3 pontoEspaco = new Vector3(-10.39f, -1.74f, 0f);
    public Vector3 pontoE = new Vector3(-44.14f, -1.74f, 0f);

    private bool jaExibidoEspaco = false;
    private bool jaExibidoE = false;
    private bool tutorialInicialConcluido = false;
    private bool dialogoFinalizado = false;

    void Start()
    {
        // Inscreve-se no evento de término do diálogo
        ConversationManager.OnConversationEnded += OnDialogoFinalizado;

        // Inicializa apenas a caixa de texto inicial
        MostrarDialogo();

        // Oculta todos os outros elementos
        OcultarTodosOsElementos();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            spriteA.sprite = spriteAPressed;
        }
        else
        {
            spriteA.sprite = spriteAIdle;
        }

        // D tecla
        if (Input.GetKey(KeyCode.D))
        {
            spriteD.sprite = spriteDPressed;
        }
        else
        {
            spriteD.sprite = spriteDIdle;
        }

        // Espaço tecla
        if (Input.GetKey(KeyCode.Space))
        {
            spriteEspaco.sprite = spriteEspacoPressed;
        }
        else
        {
            spriteEspaco.sprite = spriteEspacoIdle;
        }

        // E tecla
        if (Input.GetKey(KeyCode.E))
        {
            spriteE.sprite = spriteEPressed;
        }
        else
        {
            spriteE.sprite = spriteEIdle;
        }

        // Após o diálogo inicial, inicia o tutorial de movimento
        if (!tutorialInicialConcluido && dialogoFinalizado) 
        {
            tutorialInicialConcluido = true;
            MostrarMovimentoTeclas();
        }

        // Gerencia o tutorial do botão espaço
        if (tutorialInicialConcluido)
            GerenciarPopup(pontoEspaco, ref jaExibidoEspaco, fantasmaPopup, espaco);

        // Gerencia o tutorial do botão E
        if (jaExibidoEspaco)
            GerenciarPopup(pontoE, ref jaExibidoE, possessaoPopup, botaoE);
    }

    void MostrarMovimentoTeclas()
    {
        MostrarElemento(botaoA, true);
        MostrarElemento(botaoD, true);

        // Oculta as teclas após 5 segundos
        Invoke(nameof(OcultarMovimentoTeclas), 5f);
    }

    void GerenciarPopup(Vector3 ponto, ref bool jaExibido, GameObject popup, GameObject tecla)
    {
        // Exibe o popup ao entrar na área
        if (!jaExibido && Vector3.Distance(player.position, ponto) < 1f)
        {
            jaExibido = true;
            MostrarElemento(popup, true);
        }

        // Fecha o popup e mostra a tecla associada ao pressionar Enter
        if (popup.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            MostrarElemento(popup, false);
            MostrarElemento(tecla, true);

            // Oculta a tecla após 5 segundos
            Invoke(nameof(OcultarTeclas), 5f);
        }
    }

    void MostrarDialogo()
    {
        ConversationManager.Instance.StartConversation(dialogo);
    }

    void OnDialogoFinalizado()
    {
        // Remove a inscrição do evento para evitar chamadas múltiplas
        ConversationManager.OnConversationEnded -= OnDialogoFinalizado;

        // Marca o diálogo como finalizado
        dialogoFinalizado = true;
    }

    void MostrarElemento(GameObject elemento, bool estado)
    {
        elemento.SetActive(estado);
    }

    void OcultarTodosOsElementos()
    {
        MostrarElemento(botaoA, false);
        MostrarElemento(botaoD, false);
        MostrarElemento(fantasmaPopup, false);
        MostrarElemento(possessaoPopup, false);
        MostrarElemento(espaco, false);
        MostrarElemento(botaoE, false);
    }

    void OcultarTeclas()
    {
        MostrarElemento(espaco, false);
        MostrarElemento(botaoE, false);
    }

    void OcultarMovimentoTeclas()
    {
        MostrarElemento(botaoA, false);
        MostrarElemento(botaoD, false);
    }
}

