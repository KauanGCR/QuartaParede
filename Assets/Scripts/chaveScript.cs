using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaveScript : MonoBehaviour
{
    public Transform npc; // O gameObject que o coletável irá seguir
    public Transform fechadura; // O ponto específico onde o coletável será usado
    public float vel = 5f; // Velocidade ao seguir
    public float distMin = 3f; // Distância para "usar" o coletável
    public GameObject porta;
    public Sprite portaFechada;

    private bool isSeguindo = false;
    private bool isEntregue = false;
    public Collider2D chaveCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Checa se o coletável deve seguir o objeto
        if (collision.CompareTag("NPC")) // Certifique-se de que o GameObject tem a tag "Player"
        {
            npc = collision.transform;
            isSeguindo = true;
            Physics2D.IgnoreCollision(collision,chaveCollider, true);
        }
    }

    private void Update()
    {
        if (isSeguindo && npc != null && !isEntregue)
        {
            // Move o coletável em direção ao gameObject
            transform.position = Vector3.Lerp(transform.position, npc.position, vel * Time.deltaTime);
        }

        // Checa se chegou no ponto de entrega e está na distância correta
        if (fechadura != null && Vector3.Distance(transform.position, fechadura.position) < distMin && isSeguindo)
        {
            if (Input.GetKeyDown(KeyCode.Q)) // Interação ao pressionar 'Q'
            {
                UseCollectible();
            }
        }
    }

    private void UseCollectible()
    {
        Debug.Log("Coletável usado!");
        isEntregue = true;

        if (porta != null && portaFechada != null)
        {
            SpriteRenderer doorRenderer = porta.GetComponent<SpriteRenderer>();
            if (doorRenderer != null)
            {
                doorRenderer.sprite = portaFechada;
            }
        }

        Destroy(gameObject); // Destrói o coletável
    }
}
