using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jogadorScript : MonoBehaviour
{
    private GameObject jogadorObj;
    public reiScript reiScript;

    // Start is called before the first frame update
    void Start()
    {
        if (jogadorObj == null)
        {
            jogadorObj = GameObject.FindWithTag("Player");
            reiScript = jogadorObj.GetComponent<reiScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Verificar se o jogador está possuindo alguém ou não
        if (!reiScript.possuindo || reiScript.npcReferenciado == null)
        {
            // Seguir o jogador normalmente
            transform.position = jogadorObj.transform.position;
        }
        else
        {
            // Seguir o NPC possuído
            transform.position = reiScript.npcReferenciado.transform.position;
        }
    }
}
