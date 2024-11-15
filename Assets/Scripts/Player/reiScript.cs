using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reiScript : MonoBehaviour
{
    // Configurações de Movimento e Estamina
    public float acel = 0f;
    public float velMax = 10f;
    public float pulo = 7f;
    public float gravidade = -9.8f;
    private float velX = 0f;
    private float velY = 0f;

    public float estaminaMax = 100f;
    public float estaminaAtual;
    public float drenoEstamina = 20f;
    public bool estaminaZerada = false;
    public bool dentroParede = false;
    public bool modoFantasma = false;
    public bool possuindo = false;
    public npcScript npcAtual;
    public npcScript npcReferenciado;
    public float distMinPos = 5f;

    public Animator animator;

    public Collider2D jogadorCollider;
    //private Renderer jogadorRenderer;
    private SpriteRenderer jogadorSpriteRenderer;
    //private Sprite jogadorSpriteAnterior; // Sprite original do jogador para restaurar após a possessão

    private bool aterrado = false;
    private Collider2D paredeAtual;

    void Start()
    {
        estaminaAtual = estaminaMax;
        //jogadorRenderer = GetComponent<Renderer>();
        jogadorSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GerenciarEstamina();
        if(!possuindo)
        {
        GerenciarMovimento();
        }
        GerenciarModoFantasma();
        GerenciarPossessao();
        animator.SetBool("modoFantasma", modoFantasma);
    }

    // Controle de movimentação horizontal e pulo
    void GerenciarMovimento()
    {
        // Verifica se a tecla A ou D está pressionada
    if (Input.GetKey(KeyCode.A))
    {
        // Se o personagem estava se movendo para a direita, redefinimos a velocidade
        if (velX > 0)
            velX = 0;
        
        velX -= acel * Time.deltaTime; // Move para a esquerda
        transform.localScale = new Vector3(-1, 1, 1); // Vira para a esquerda
    }
    else if (Input.GetKey(KeyCode.D))
    {
        // Se o personagem estava se movendo para a esquerda, redefinimos a velocidade
        if (velX < 0)
            velX = 0;

        velX += acel * Time.deltaTime; // Move para a direita
        transform.localScale = new Vector3(1, 1, 1); // Vira para a direita
    }
    else
    {
        // Aplica uma desaceleração rápida para parar o personagem
        float desaceleracao = 15f;
        velX = Mathf.MoveTowards(velX, 0, desaceleracao * Time.deltaTime);
    }
    

        float velocidadeAtual = (dentroParede && modoFantasma) ? velMax * 0.5f : velMax; 
        velX = Mathf.Clamp(velX, -velocidadeAtual, velocidadeAtual);

    // Atualiza a direção da sprite
        if (velX > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Virado para a direita
        }
        else if (velX < 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Virado para a esquerda
        }
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

    // Controle da estamina e lógica do modo fantasma
    void GerenciarEstamina()
    {
        // Impede estamina negativa
        if (estaminaAtual < 0)
            estaminaAtual = 0;

        // Regenera estamina se não está no modo fantasma e não está possuindo
        if (!modoFantasma && !possuindo)
        {
            estaminaAtual = Mathf.Min(estaminaAtual + drenoEstamina * Time.deltaTime, estaminaMax);
            if(estaminaAtual >= 100)
            {
                estaminaZerada = false;
            }
        }
    }

    void GerenciarModoFantasma()
    {
        // Ativa o modo fantasma se a barra de espaço está pressionada e ainda tem estamina
        modoFantasma = Input.GetKey(KeyCode.Space) && !estaminaZerada;

        // Expulsa da parede caso o modo fantasma seja desativado
        if (dentroParede && !modoFantasma)
        {
            ExpulsarParede();
        }

        // Consumo de estamina no modo fantasma
        if (modoFantasma)
        {
            estaminaAtual -= drenoEstamina * Time.deltaTime;

            // Expulsa da parede se a estamina acabar
            if (estaminaAtual <= 0)
            {
                modoFantasma = false;
                estaminaZerada = true;
            }
        }
    }

    // Controle de possessão de NPCs
    void GerenciarPossessao()
    {
        if (possuindo)
        {
            // Libera o NPC se a estamina acabar, a tecla 'E' for pressionada ou sair do modo fantasma
            if (estaminaAtual <= 0 || Input.GetKeyDown(KeyCode.E) || !modoFantasma)
            {
                LiberarNPC();
            }
        }
        else
        {
            // Possui o NPC se estiver no modo fantasma e próximo a um NPC
            if (modoFantasma && Input.GetKeyDown(KeyCode.E) && npcAtual != null &&
                Vector2.Distance(transform.position, npcAtual.transform.position) <= distMinPos)
            {
                PossuirNPC();
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("objeto") && modoFantasma && !possuindo)
        {
            dentroParede = true;
            paredeAtual = collision;
            Physics2D.IgnoreCollision(collision, jogadorCollider, true);
        }
        if (collision.CompareTag("NPC"))
        {
            npcAtual = collision.GetComponent<npcScript>();
            Physics2D.IgnoreCollision(collision, jogadorCollider, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("objeto"))
        {
            dentroParede = false;
            paredeAtual = null;
            Physics2D.IgnoreCollision(collision, jogadorCollider, false);
        }
        if (collision.CompareTag("NPC"))
        {
            npcAtual = null;
            Physics2D.IgnoreCollision(collision, jogadorCollider, false);
        }
    }

    private void PossuirNPC()
    {
        if (npcAtual != null)
        {
            npcAtual.Possuir();
            possuindo = true;
            jogadorSpriteRenderer.enabled = false;
            npcReferenciado = npcAtual;
            transform.position = npcAtual.transform.position;
        }
    }

    private void LiberarNPC()
    {
        if (npcReferenciado != null)
        {
            npcReferenciado.Liberar();
            possuindo = false;
            modoFantasma = false;
            //jogadorSpriteRenderer.sprite = jogadorSpriteAnterior; // Restaura o sprite original
            jogadorSpriteRenderer.enabled = true; // Reaparece o jogador
            npcReferenciado = null;
        }
    }

    private void ExpulsarParede()
    {
        if (paredeAtual != null)
        {
            Vector3 direcao = (transform.position.x - paredeAtual.bounds.center.x) >= 0 ? Vector3.right : Vector3.left;
            float distanciaX = paredeAtual.bounds.extents.x + jogadorCollider.bounds.extents.x;
            transform.position = new Vector3(
                paredeAtual.bounds.center.x + direcao.x * (distanciaX + 0.5f),
                transform.position.y,
                transform.position.z
            );
        }
        dentroParede = false;
    }
}

