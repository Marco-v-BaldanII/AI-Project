using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreen : MonoBehaviour
{
    
    public void ToTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
