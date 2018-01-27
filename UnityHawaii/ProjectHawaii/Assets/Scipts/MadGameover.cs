using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MadGameover : MonoBehaviour {

    [SerializeField] Text timeText;

	// Use this for initialization
	void Start () {
        timeText.text += PlayerPrefs.GetString("playTime", "None");
    }
	

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }
	
}
