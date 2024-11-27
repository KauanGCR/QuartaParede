using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class npcScript : MonoBehaviour
{
    public float acel = 10f;
    public float velMax = 5f;
    public float pulo = 15f;
    public float gravidade = -9.8f;
    private float velX = 0f;
    private float velY = 0f;
    private bool aterrado = false;
    public bool isPossessed = false;
    public bool isPatrolling = false;
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;
    private bool movingToB = true;
    private Collider2D npcCollider;
    public Animator animator;
    public bool isScriptedNPC = false;
    public NPCConversation dialogo;
    private bool dialogueStarted = false; // Flag para garantir que o diálogo só inicie uma vez
    public bool freeze;

    void Start()
    {
        npcCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (isPossessed && isScriptedNPC && !dialogueStarted)
        {
            IniciarCenaScriptada();
            return; // Evita que o NPC tente se mover enquanto o diálogo ocorre
        }

        animator.SetFloat("velX", Mathf.Abs(velX));

        if (isPossessed && !isScriptedNPC && !freeze)
        {
            GerenciarMovimento();
        }
        else if (isPatrolling)
        {
            Patrulhar();
        }
        else
        {
            velX = 0;
            animator.SetFloat("velX", 0);
        }

        animator.SetBool("isPossuido", isPossessed);
    }

    void GerenciarMovimento()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (velX > 0) velX = 0;
            velX -= acel * Time.deltaTime;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (velX < 0) velX = 0;
            velX += acel * Time.deltaTime;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            float desaceleracao = 15f;
            velX = Mathf.MoveTowards(velX, 0, desaceleracao * Time.deltaTime);
        }

        velX = Mathf.Clamp(velX, -velMax, velMax);

        if (Input.GetKeyDown(KeyCode.W) && aterrado)
        {
            velY = pulo;
            aterrado = false;
        }

        if (!aterrado)
            velY += gravidade * Time.deltaTime;

        GetComponent<Rigidbody2D>().velocity = new Vector2(velX, velY);
    }

    void Patrulhar()
    {
        float step = velMax * Time.deltaTime;
        Vector2 target = movingToB ? patrolPointB : patrolPointA;

        transform.position = Vector2.MoveTowards(transform.position, target, step);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            movingToB = !movingToB;
        }

        float direction = (target.x - transform.position.x);
        transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);

        animator.SetFloat("velX", Mathf.Abs(velMax));
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

    public void Possuir()
    {
        isPossessed = true;
    }

    public void Liberar()
    {
        isPossessed = false;
    }

    public bool MoverParaPonto(Vector3 pontoFinal)
    {
        if (isPossessed) return false; // Não mover se o NPC estiver possuído

        float step = velMax * Time.deltaTime; // Velocidade do movimento

        // Log para verificar os valores
        Debug.Log($"Movendo NPC para {pontoFinal}. Posição atual: {transform.position}");

        transform.position = Vector2.MoveTowards(transform.position, pontoFinal, step);

        // Verifica se chegou ao destino
        if (Vector2.Distance(transform.position, pontoFinal) < 0.1f)
        {
            velX = 0; // Para a animação
            animator.SetFloat("velX", 0);
            Debug.Log("NPC chegou ao destino.");
            return true;
        }

        // Atualiza direção e animação
        float direction = (pontoFinal.x - transform.position.x);
        transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);
        animator.SetFloat("velX", Mathf.Abs(velMax));

        return false;
    }

    void IniciarCenaScriptada()
    {
        dialogueStarted = true;
        velX = 0;
        animator.SetFloat("velX", 0);
        Debug.Log("Iniciou conversa");
        ConversationManager.Instance.StartConversation(dialogo);
    }
}
