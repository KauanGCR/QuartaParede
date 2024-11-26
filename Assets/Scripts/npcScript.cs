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
    public bool isPatrolling = false; // Para verificar se o NPC deve patrulhar
    public Vector2 patrolPointA; // Ponto de início da patrulha
    public Vector2 patrolPointB; // Ponto final da patrulha
    private bool movingToB = true; // Para alternar entre os pontos de patrulha
    private Collider2D npcCollider;
    public Animator animator;
    public bool freeze;

    void Start()
    {
        npcCollider = GetComponent<Collider2D>(); // Acessa o Collider do NPC
    }

    void Update()
    {
        animator.SetFloat("velX", Mathf.Abs(velX));

        if (isPossessed && !freeze)
        {
            GerenciarMovimento();
        }
        else if (isPatrolling)
        {
            Patrulhar();
        }
        else
        {
            // Garante que a velocidade e animação sejam zeradas se o NPC não estiver patrulhando ou possuído
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
            velX -= acel * Time.deltaTime; // Move para a esquerda
            transform.localScale = new Vector3(-1, 1, 1); // Vira para a esquerda
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (velX < 0) velX = 0;
            velX += acel * Time.deltaTime; // Move para a direita
            transform.localScale = new Vector3(1, 1, 1); // Vira para a direita
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
        float step = velMax * Time.deltaTime; // Velocidade da patrulha
        Vector2 target = movingToB ? patrolPointB : patrolPointA;

        // Movimenta o NPC em direção ao ponto de patrulha
        transform.position = Vector2.MoveTowards(transform.position, target, step);

        // Verifica se chegou ao destino e alterna o ponto de patrulha
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            movingToB = !movingToB;
        }

        // Define a direção do sprite com base no próximo ponto de patrulha
        float direction = (target.x - transform.position.x);
        transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);

        // Ajusta o parâmetro da animação com a velocidade máxima
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
}
