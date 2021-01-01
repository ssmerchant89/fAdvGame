using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{

    public void restart(){
        SceneManager.UnloadScene("Fairy_Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
