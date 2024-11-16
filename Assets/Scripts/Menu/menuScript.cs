using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public void Jogar()
    {
        SceneManager.LoadScene("Cutscene");
    }

    public void Sair()
    {
        Debug.Log("Sair do jogo");
        Application.Quit();
    }
}
