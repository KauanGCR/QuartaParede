using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class cutsceneScript : MonoBehaviour
{
    public Image cutsceneImage; // Arraste o componente Image aqui
    public TMP_Text cutsceneText; // Arraste o componente Text aqui (ou TextMeshPro, se estiver usando TMP)
    public Image textBoxBackground; // Caixa de fundo do texto (Image para o background da caixa)
    public GameObject arrowIndicator; // Seta de feedback para o jogador
    public Sprite[] images; // Arraste as imagens da cutscene para este array
    public string[] texts; // Adicione o texto correspondente a cada imagem aqui
    public float zoomDuration = 3f; // Tempo de zoom para cada imagem
    public float fadeDuration = 1f; // Tempo de fade in/out
    public float displayTime = 2f; // Tempo que cada imagem fica visível antes do fade
    public float textFadeDelay = 0.5f; // Atraso no fade do texto e da caixa após a imagem

    private void Start()
    {
        arrowIndicator.SetActive(false); // Garante que a seta está desativada no início
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        int lastIndexWithoutEffects = images.Length - 3;

        for (int i = 0; i < images.Length; i++)
        {
            // Troca a imagem atual
            cutsceneImage.sprite = images[i];
            cutsceneImage.color = new Color(1, 1, 1, 0); // Transparente no início

            // Atualiza o texto e a caixa de texto
            cutsceneText.text = texts[i];
            cutsceneText.color = new Color(1, 1, 1, 0);
            textBoxBackground.color = new Color(1, 1, 1, 0);

            // Reseta o zoom
            ResetZoom();

            // Fade In da imagem
            yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

            // Aguarda antes de iniciar o fade do texto e da caixa de texto
            yield return new WaitForSeconds(textFadeDelay);

            // Fade In do texto e da caixa de texto
            yield return StartCoroutine(FadeTextAndBox(0, 1, fadeDuration));

            // Aplica zoom se não for uma das últimas imagens
            if (i < lastIndexWithoutEffects)
            {
                yield return StartCoroutine(ZoomImage(zoomDuration));
            }

            // Ativa a seta para indicar que o jogador pode prosseguir
            arrowIndicator.SetActive(true);

            // Espera pelo pressionamento da tecla Enter
            yield return StartCoroutine(WaitForEnterPress());

            // Desativa a seta
            arrowIndicator.SetActive(false);

            // Fade Out sincronizado
            yield return StartCoroutine(FadeElementsOut(fadeDuration));
        }

        // Finaliza a cutscene
        EndCutscene();
    }

    private IEnumerator WaitForEnterPress()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;
        }
    }

    private IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            cutsceneImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeTextAndBox(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            cutsceneText.color = new Color(1, 1, 1, alpha); // Fade do texto
            textBoxBackground.color = new Color(1, 1, 1, alpha); // Fade da caixa de texto
            yield return null;
        }
    }

    private IEnumerator FadeElementsOut(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, time / duration);

            // Sincroniza o fade out de todos os elementos
            cutsceneImage.color = new Color(1, 1, 1, alpha);
            cutsceneText.color = new Color(1, 1, 1, alpha);
            textBoxBackground.color = new Color(1, 1, 1, alpha);

            yield return null;
        }
    }

    private IEnumerator ZoomImage(float duration)
    {
        RectTransform rect = cutsceneImage.rectTransform;
        Vector3 initialScale = rect.localScale;
        Vector3 targetScale = initialScale * 1.2f;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            rect.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }
    }

    private void ResetZoom()
    {
        RectTransform rect = cutsceneImage.rectTransform;
        rect.localScale = Vector3.one;
    }

    private void EndCutscene()
    {
        Debug.Log("Cutscene finalizada!");
        SceneManager.LoadScene("Fase1");
    }
}
