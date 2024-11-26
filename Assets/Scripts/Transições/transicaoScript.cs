using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class transicaoScript : MonoBehaviour
{
    public Image fadeImage; // Imagem usada para o fade
    public float fadeDuration = 1.0f; // Duração do fade
    private Vector3 pontoAtivacao = new Vector3(-58f, -1.74f, 0f); // Posição que ativa o fim da fase
    public Transform player; // Referência ao jogador
    public string proximaCena; // Nome da próxima cena

    private bool transitioning = false; // Evita transições duplicadas

    private void Start()
    {
        if (fadeImage == null || player == null)
        {
            Debug.LogError("Configuração incompleta no Phase1Manager.");
            return;
        }

        fadeImage.color = new Color(0, 0, 0, 1); // Garante que a tela comece preta
        fadeImage.gameObject.SetActive(true); // Ativa o objeto da imagem
        StartCoroutine(FadeIn()); // Começa o fade-in ao carregar a fase
    }

    private void Update()
    {
        if (transitioning) return; // Evita múltiplas transições

        // Verifica se o jogador atingiu o ponto de ativação
        if (Vector3.Distance(player.position, pontoAtivacao) < 1f)
        {
            Debug.Log("Chegou");
            StartCoroutine(TransitionToNextScene());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (elapsed / fadeDuration));
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
        fadeImage.gameObject.SetActive(false); // Oculta a imagem após o fade-in
    }

    private IEnumerator FadeOut()
    {
        fadeImage.gameObject.SetActive(true); // Garante que a imagem está ativa
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, elapsed / fadeDuration);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 1);
    }

    private IEnumerator TransitionToNextScene()
    {
        transitioning = true; // Evita transições repetidas
        yield return FadeOut(); // Faz o fade para preto

        // Carrega a próxima cena
        if (!string.IsNullOrEmpty(proximaCena))
        {
            SceneManager.LoadScene(proximaCena);
        }
        else
        {
            Debug.LogError("A próxima cena não foi configurada.");
        }
    }
}
