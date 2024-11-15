using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcScript : MonoBehaviour
{
    public float acel = 10f;
    public float velMax = 5f;
    public float pulo = 15f;
    public float gravidade = -9.8f;
    private float velX = 0f;
    private float velY = 0f;
    private bool aterrado = false;
    public bool isPossessed = false; // Para verificar se o NPC está possuído

    //private Renderer npcRenderer;
    private Collider2D npcCollider;

    void Start()
    {
        //npcRenderer = GetComponent<Renderer>(); // Acessa o Renderer do NPC
        npcCollider = GetComponent<Collider2D>(); // Acessa o Collider do NPC
    }

    // Método opcional, caso você queira adicionar comportamentos ao NPC quando ele não for possuído
    void Update()
    {
        if (isPossessed)
        {
           GerenciarMovimento();
        }
        else
        {
            
        }
    }

    void GerenciarMovimento()
    {
        // Movimentação Horizontal
        if (Input.GetKey(KeyCode.A))
        {
            velX -= acel * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velX += acel * Time.deltaTime;
        }
        else
        {
            velX = Mathf.Lerp(velX, 0, 5 * Time.deltaTime); // Suaviza a desaceleração
        }

        velX = Mathf.Clamp(velX, -velMax, velMax);

        // Pulo
        if (Input.GetKeyDown(KeyCode.W) && aterrado)
        {
            velY = pulo;
            aterrado = false;
        }

        // Aplicação da gravidade
        if (!aterrado)
            velY += gravidade * Time.deltaTime;

        // Atualiza a posição do jogador
        GetComponent<Rigidbody2D>().velocity = new Vector2(velX, velY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("chao"))
        {
            aterrado = true;
            velY = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("chao"))
        {
            aterrado = false;
        }
    }

    // Este método pode ser chamado do script do jogador para indicar que o NPC foi possuído
    public void Possuir()
    {
        isPossessed = true;
        //npcRenderer.enabled = false; // Torna o NPC invisível
        //npcCollider.enabled = false; // Desativa a colisão do NPC
    }

    // Este método pode ser chamado para liberar a possessão
    public void Liberar()
    {
        isPossessed = false;
        //npcRenderer.enabled = true; // Torna o NPC visível novamente
    }

}
