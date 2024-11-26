using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class transicaoFasesScripts : MonoBehaviour
{
    public Image fadeImage; // Imagem usada para o fade
    public float fadeDuration = 1.0f; // Duração do fade
    public string proximaCena; // Nome da próxima cena

    private bool transitioning = false; // Evita múltiplas transições

    private void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Configuração incompleta no PhaseWithDialogueManager.");
            return;
        }

        fadeImage.color = new Color(0, 0, 0, 1); // Garante que a tela comece preta
        fadeImage.gameObject.SetActive(true); // Ativa o objeto da imagem
        StartCoroutine(FadeIn()); // Começa o fade-in ao carregar a fase
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
        transitioning = true; // Evita múltiplas transições
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

    // Função chamada pelo evento do Dialogue Editor
    public void OnDialogueEnd()
    {
        if (transitioning) return; // Evita múltiplas transições
        StartCoroutine(TransitionToNextScene());
    }
}
