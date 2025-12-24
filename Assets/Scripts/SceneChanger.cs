using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName); //carga una escena
    }

}
