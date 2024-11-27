using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DialogueEditor;

public class TavernaInteracao : MonoBehaviour
{
    public Transform npc; // Referência ao NPC que o jogador deve possuir
    public Transform pontoInteracao; // Ponto específico para a interação
    public float distMin = 3f; // Distância mínima para interação
    public Image fadeImage; // Imagem para o fade
    public float fadeDuration = 1.0f; // Duração do fade
    public float waitTime = 1.0f; // Tempo de espera no preto
    public NPCConversation dialogo; // Referência ao Dialogue Editor

    private bool isPossessed = false;

    private void Update()
    {
        // Checa se o NPC está possuído e dentro da distância de interação
        if (isPossessed && pontoInteracao != null && Vector3.Distance(npc.position, pontoInteracao.position) < distMin)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(HandleInteraction());
            }
        }
    }

    private IEnumerator HandleInteraction()
    {
        yield return FadeOut();
        yield return new WaitForSeconds(waitTime);
        yield return FadeIn();

        // Inicia o diálogo
        if (dialogo != null)
        {
            ConversationManager.Instance.StartConversation(dialogo);
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0;
        fadeImage.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (elapsed / fadeDuration));
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0;
        fadeImage.gameObject.SetActive(true);

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, elapsed / fadeDuration);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o NPC entrou na área e está possuído
        if (collision.CompareTag("NPC"))
        {
            npcScript npcController = collision.GetComponent<npcScript>();
            if (npcController != null && npcController.isPossessed)
            {
                isPossessed = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            isPossessed = false;
        }
    }
}
