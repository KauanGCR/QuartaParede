using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class buttonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSprite;  // Sprite normal do botão
    public Sprite hoverSprite;   // Sprite quando o mouse estiver sobre o botão
    private Image buttonImage;   // A imagem do botão

    void Start()
    {
        // Obtém a referência da imagem do botão
        buttonImage = GetComponent<Image>();

        // Define o sprite inicial (normal)
        buttonImage.sprite = normalSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Muda para o sprite de hover quando o mouse entra no botão
        buttonImage.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Retorna ao sprite normal quando o mouse sai do botão
        buttonImage.sprite = normalSprite;
    }
}
