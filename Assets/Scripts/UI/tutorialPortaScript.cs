using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPortaScript : MonoBehaviour
{
    [Header("Configurações das Teclas")]
    public GameObject teclaW; // GameObject da tecla W
    public SpriteRenderer spriteW;
    public Sprite spriteWIdle;
    public Sprite spriteWPressed;

    public GameObject teclaQ; // GameObject da tecla Q
    public SpriteRenderer spriteQ;
    public Sprite spriteQIdle;
    public Sprite spriteQPressed;

    [Header("Posições de Ativação")]
    public Vector3 pontoW = new Vector3(-13.5f, -3.75f, 0f);
    public Vector3 pontoQ = new Vector3(-1.75f, -3.75f, 0f);

    [Header("Player")]
    public Transform player; // Posição do jogador

    [Header("NPC")]
    public Transform npc; // Posição do NPC (aqui você faz a referência ao NPC)

    private bool jaExibidoW = false;
    private bool jaExibidoQ = false;

    void Start()
    {
        // Inicializa as teclas como ocultas
        teclaW.SetActive(false);
        teclaQ.SetActive(false);
    }

    void Update()
    {
        // Exibe a tecla W apenas na primeira vez que o jogador entrar na área de ativação
        if (!jaExibidoW && EstaProximo(player.position, pontoW))
        {
            ExibirTecla(teclaW, ref jaExibidoW);
        }

        // A tecla Q só será exibida após a tecla W ter sido exibida e o NPC entrar na área de ativação da tecla Q
        if (jaExibidoW && !jaExibidoQ && EstaProximo(npc.position, pontoQ)) // Usando a posição do NPC aqui
        {
            ExibirTecla(teclaQ, ref jaExibidoQ);
        }

        // Animação das teclas: atualiza a sprite dependendo se a tecla está sendo pressionada
        AtualizarSpriteTecla(KeyCode.W, spriteW, spriteWIdle, spriteWPressed);
        AtualizarSpriteTecla(KeyCode.Q, spriteQ, spriteQIdle, spriteQPressed);
    }

    // Função para verificar a proximidade de um objeto (agora com opção de verificar tanto jogador quanto NPC)
    bool EstaProximo(Vector3 objetoPos, Vector3 ponto)
    {
        return Vector3.Distance(objetoPos, ponto) < 1f;
    }

    // Exibe a tecla correspondente e marca como exibida
    void ExibirTecla(GameObject tecla, ref bool jaExibido)
    {
        tecla.SetActive(true); // Torna o GameObject da tecla visível
        jaExibido = true; // Marca a tecla como já exibida
        Invoke(nameof(OcultarTecla), 5f); // Oculta a tecla após 5 segundos
    }

    // Oculta as teclas após 5 segundos, garantindo que elas não reapareçam
    void OcultarTecla()
    {
        if (jaExibidoW)
        {
            teclaW.SetActive(false); // Esconde a tecla W
        }

        if (jaExibidoQ)
        {
            teclaQ.SetActive(false); // Esconde a tecla Q
        }
    }

    // Atualiza a animação da tecla, trocando entre o estado normal e pressionado
    void AtualizarSpriteTecla(KeyCode tecla, SpriteRenderer spriteRenderer, Sprite idle, Sprite pressed)
    {
        if (spriteRenderer.gameObject.activeSelf) // Só atualiza se a tecla estiver visível
        {
            spriteRenderer.sprite = Input.GetKey(tecla) ? pressed : idle; // Troca o sprite dependendo da pressão da tecla
        }
    }
}
