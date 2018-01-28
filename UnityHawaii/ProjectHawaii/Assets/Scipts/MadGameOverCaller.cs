using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MadGameOverCaller : MonoBehaviour {


	void Start () {
        EventManager.OnGameOver += Gameover;
	}


    void Gameover()
    {
        var timespan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        var timy = timespan.ToString(@"mm\:ss");
        PlayerPrefs.SetString("playTime", timy);
        SceneManager.LoadScene(2);
    }
	
	
}
