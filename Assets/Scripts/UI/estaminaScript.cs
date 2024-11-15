using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class estaminaScript : MonoBehaviour
{
    [SerializeField] private Image BarraEstamina; 
    [SerializeField] private Image EstaminaVerde;
    [SerializeField] private Image EstaminaVermelha;  
    public GameObject estamina;
    public GameObject cameraObj;
    public GameObject jogadorObj;
    public reiScript jogadorScript;
    public float tempoInatividade;
    public float tempoParaOcultar = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        if (jogadorObj == null)
        {
            jogadorObj = GameObject.FindWithTag("Player");
            jogadorScript = jogadorObj.GetComponent<reiScript>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        estamina.transform.position = new Vector3(cameraObj.transform.position.x + 1.5f, cameraObj.transform.position.y + 2f);

        if (jogadorObj != null && EstaminaVerde != null && EstaminaVermelha != null)
        {
            if(jogadorScript.modoFantasma)
            {
                EstaminaVermelha.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax + 0.07f);
            }
            else
            {
            EstaminaVermelha.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax);
            }
        }

        if(jogadorScript.estaminaZerada)
        {
            EstaminaVerde.enabled = false;
        }
        else
        {
            EstaminaVerde.enabled = true;
        }
        
        if (jogadorScript.estaminaAtual < jogadorScript.estaminaMax && !jogadorScript.estaminaZerada)
            {
                tempoInatividade = 0f;
                BarraEstamina.enabled = true;
                EstaminaVerde.enabled = true;
                EstaminaVermelha.enabled = true;
            }
            else
            {
                // Incrementar o tempo de inatividade e ocultar se necessário
                tempoInatividade += Time.deltaTime;
                if (tempoInatividade >= tempoParaOcultar && !jogadorScript.estaminaZerada)
                {
                    BarraEstamina.enabled = false;
                    EstaminaVerde.enabled = false;
                    EstaminaVermelha.enabled = false;
                }
            }
        EstaminaVerde.fillAmount = (jogadorScript.estaminaAtual / jogadorScript.estaminaMax);

    }
}
