using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityScript : MonoBehaviour
{
    public void SetQuality(int quality){
	QualitySettings.SetQualityLevel(quality,true);
    }

    public void QuitGame(){
	Application.Quit();
    }

    public void RestartLevel(){
	Time.timeScale = 1;
	Application.LoadLevel("World");
    }
}
