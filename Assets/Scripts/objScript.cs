using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objScript : MonoBehaviour
{
    [SerializeField] private Sprite normalSprite; // Sprite padrão
    [SerializeField] private Sprite fantasmaSprite; // Sprite no modo fantasma

    private SpriteRenderer spriteRenderer; // Referência ao SpriteRenderer do objeto
    private GameObject jogadorObj;
    public reiScript jogadorScript;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        jogadorObj = GameObject.FindWithTag("Player");
        if (jogadorObj != null)
        {
            jogadorScript = jogadorObj.GetComponent<reiScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jogadorScript != null)
        {
            UpdateSprite(jogadorScript.modoFantasma);
        }
    }

    void UpdateSprite(bool modoFantasma)
    {
    if (spriteRenderer != null)
        {
            spriteRenderer.sprite = modoFantasma ? fantasmaSprite : normalSprite;
        }
    }
}
