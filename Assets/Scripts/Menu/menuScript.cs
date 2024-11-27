using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public GameObject PanelCredits;
    public void Jogar()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void Creditos()
    {
        PanelCredits.SetActive(!PanelCredits.activeSelf);
    }

    public void Sair()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }
}
