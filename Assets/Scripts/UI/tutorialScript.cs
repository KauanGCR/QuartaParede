using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [Header("Elementos")]
    public GameObject caixaTexto;
    public GameObject imagemRei;
    public GameObject botaoA;
    public GameObject botaoD;
    public GameObject fantasmaPopup;
    public GameObject possessaoPopup;
    public GameObject espaco;
    public GameObject botaoE;

    [Header("Player")]
    public Transform player;

    [Header("Offsets")]
    public Vector3 offsetBotaoA = new Vector3(-10f, 0.5f, 0f); // Deslocamento relativo ao jogador para o botão A
    public Vector3 offsetBotaoD = new Vector3(10f, 0.5f, 0f);  // Deslocamento relativo ao jogador para o botão D

    private bool moveu = false;
    private bool jaExibidoEspaco = false;
    private bool jaExibidoE = false;

    private Vector3 pontoMovimento = new Vector3(0.93f, -1.74f, 0f);
    private Vector3 pontoEspaco = new Vector3(-10.39f, -1.74f, 0f);
    private Vector3 pontoE = new Vector3(-43.14f, -1.74f, 0f);

    private float tempoAfastadoEspaco = 0f;
    private bool temporizadorEspacoAtivo = false;
    private float tempoAfastadoE = 0f;
    private bool temporizadorEAtivo = false;

    void Start()
    {
        ShowElement(caixaTexto, true);
        ShowElement(imagemRei, true);
        ShowElement(botaoA, false);
        ShowElement(botaoD, false);
        ShowElement(fantasmaPopup, false);
        ShowElement(espaco, false);
        ShowElement(possessaoPopup, false);
        ShowElement(botaoE, false);
    }

    void Update()
    {
        // Atualiza posição dos botões de movimento enquanto estão visíveis
        if (botaoA.activeSelf)
        {
            botaoA.transform.position = player.position + offsetBotaoA;
        }

        if (botaoD.activeSelf)
        {
            botaoD.transform.position = player.position + offsetBotaoD;
        }

        // Fase 1: Fecha diálogo do rei
        if (caixaTexto.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ShowElement(caixaTexto, false);
            ShowElement(imagemRei, false);
            ShowElement(botaoA, true);
            ShowElement(botaoD, true);
        }

        // Fase 2: Verifica movimento do jogador
        if (!moveu && Vector3.Distance(player.position, pontoMovimento) > 1f)
        {
            moveu = true;
            Invoke(nameof(EsconderBotoesMov), 2f);
        }

        // Fase 3: Lógica do botão espaço
        GerenciarPopup(pontoEspaco, ref jaExibidoEspaco, fantasmaPopup, espaco, ref temporizadorEspacoAtivo, ref tempoAfastadoEspaco);

        // Fase 4: Lógica do botão E
        GerenciarPopup(pontoE, ref jaExibidoE, possessaoPopup, botaoE, ref temporizadorEAtivo, ref tempoAfastadoE);

        // Verifica retorno para posição inicial
        if (moveu && Vector3.Distance(player.position, pontoMovimento) < 1f)
        {
            ShowElement(botaoA, true);
            ShowElement(botaoD, true);
            Invoke(nameof(EsconderBotoesMov), 2f);
        }
    }

    void GerenciarPopup(Vector3 ponto, ref bool jaExibido, GameObject popup, GameObject botao, ref bool temporizadorAtivo, ref float tempoAfastado)
    {
        // Exibe pop-up
        if (!jaExibido && Vector3.Distance(player.position, ponto) < 1f)
        {
            jaExibido = true;
            ShowElement(popup, true);
        }

        // Fecha pop-up e mostra botão
        if (popup.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ShowElement(popup, false);
            ShowElement(botao, true);
        }

        // Esconde botão se jogador se afastar
        if (jaExibido && Vector3.Distance(player.position, ponto) > 1f)
        {
            if (!temporizadorAtivo)
            {
                temporizadorAtivo = true;
                tempoAfastado = Time.time;
            }
            else if (Time.time - tempoAfastado >= 1.5f)
            {
                ShowElement(botao, false);
            }
        }

        // Reaparece botão ao retornar
        if (Vector3.Distance(player.position, ponto) < 1f && jaExibido)
        {
            ShowElement(botao, true);
            temporizadorAtivo = false;
        }
    }

    void ShowElement(GameObject element, bool show)
    {
        element.SetActive(show);
    }

    void EsconderBotoesMov()
    {
        ShowElement(botaoA, false);
        ShowElement(botaoD, false);
    }
}
