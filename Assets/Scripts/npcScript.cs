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

    void Start()
    {
        npcCollider = GetComponent<Collider2D>(); // Acessa o Collider do NPC
    }

    void Update()
    {
        animator.SetFloat("velX", Mathf.Abs(velX));
        if (isPossessed)
        {
            GerenciarMovimento();
        }
        else if (isPatrolling)
        {
            animator.SetFloat("velX", Mathf.Abs(velMax));
            Patrulhar();
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

        if (movingToB)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPointB, step);
            if (Vector2.Distance(transform.position, patrolPointB) < 0.1f) movingToB = false;
            transform.localScale = new Vector3(1, 1, 1); // Virado para a direita
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPointA, step);
            if (Vector2.Distance(transform.position, patrolPointA) < 0.1f) movingToB = true;
            transform.localScale = new Vector3(-1, 1, 1); // Virado para a esquerda
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

    public void Possuir()
    {
        isPossessed = true;
    }

    public void Liberar()
    {
        isPossessed = false;
    }
}
