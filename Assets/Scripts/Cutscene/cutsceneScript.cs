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
    public Sprite[] images; // Arraste as imagens da cutscene para este array
    public string[] texts; // Adicione o texto correspondente a cada imagem aqui
    public float zoomDuration = 3f; // Tempo de zoom para cada imagem
    public float fadeDuration = 1f; // Tempo de fade in/out
    public float displayTime = 2f; // Tempo que cada imagem fica visível antes do fade

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Determina a quantidade de imagens para não aplicar efeitos
        int lastIndexWithoutEffects = images.Length - 3;

        for (int i = 0; i < images.Length; i++)
        {
            // Troca a imagem atual
            cutsceneImage.sprite = images[i];
            cutsceneImage.color = new Color(1, 1, 1, 0); // Transparente no início

            // Atualiza o texto da caixa de texto
            cutsceneText.text = texts[i]; // Atribui o texto da array de acordo com o índice
            cutsceneText.color = new Color(1, 1, 1, 0); // Texto começa invisível

            // Faz a caixa de fundo do texto ficar invisível inicialmente
            textBoxBackground.color = new Color(1, 1, 1, 0); // Caixa de texto invisível inicialmente

            // Se a imagem atual não for uma das 3 últimas, aplica o efeito de fade e zoom
            if (i < lastIndexWithoutEffects)
            {
                // Reseta a escala da imagem antes de aplicar o zoom
                ResetZoom();

                // Fade In para imagem
                yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

                // Fade In para o texto
                yield return StartCoroutine(FadeText(0, 1, fadeDuration));

                // Fade In para a caixa de fundo do texto
                yield return StartCoroutine(FadeImageBackground(0, 1, fadeDuration));

                // Zoom In
                yield return StartCoroutine(ZoomImage(zoomDuration));
            }
            else
            {
                // Para as 3 últimas imagens, apenas faz o fade in para a imagem
                yield return StartCoroutine(FadeImage(0, 1, fadeDuration));

                // Faz o fade in para o texto
                yield return StartCoroutine(FadeText(0, 1, fadeDuration));

                // Faz o fade in para a caixa de fundo do texto
                yield return StartCoroutine(FadeImageBackground(0, 1, fadeDuration));
            }

            // Espera o tempo de exibição ou o pressionamento da tecla Enter
            yield return new WaitForSeconds(displayTime);

            // Para todas as imagens, faz o fade out do texto, da imagem e da caixa de fundo
            yield return StartCoroutine(FadeImage(1, 0, fadeDuration));
            yield return StartCoroutine(FadeText(1, 0, fadeDuration));
            yield return StartCoroutine(FadeImageBackground(1, 0, fadeDuration));
        }

        // Finaliza a cutscene
        EndCutscene();
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

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            cutsceneText.color = new Color(1, 1, 1, alpha); // Modifica a transparência do texto
            yield return null;
        }
    }

    private IEnumerator FadeImageBackground(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            textBoxBackground.color = new Color(1, 1, 1, alpha); // Modifica a transparência do fundo
            yield return null;
        }
    }

    private IEnumerator ZoomImage(float duration)
    {
        RectTransform rect = cutsceneImage.rectTransform;
        Vector3 initialScale = rect.localScale; // Obtém a escala inicial
        Vector3 targetScale = initialScale * 1.2f; // Aumenta o tamanho 20% (ajuste se necessário)

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            rect.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }
    }

    // Função para resetar o zoom (escala inicial)
    private void ResetZoom()
    {
        RectTransform rect = cutsceneImage.rectTransform;
        rect.localScale = Vector3.one; // Reseta a escala para o valor original (sem zoom)
    }

    // Função que espera o pressionamento da tecla "Enter"
    private IEnumerator WaitForEnterPress()
    {
        // Espera até que a tecla "Enter" seja pressionada
        while (!Input.GetKeyDown(KeyCode.Return)) // KeyCode.Return é a tecla "Enter"
        {
            yield return null; // Aguarda até que a tecla seja pressionada
        }
    }

    private void EndCutscene()
    {
        // Aqui você pode trocar para a próxima Scene ou fazer outra ação
        Debug.Log("Cutscene finalizada!");
        SceneManager.LoadScene("Fase1");
        // Exemplo de troca de scene:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("ProximaScene");
    }
}
