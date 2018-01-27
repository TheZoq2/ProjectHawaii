using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderInfo : MonoBehaviour
{

    [SerializeField]
    private int _position = 0;
    private Slider _slider = null;

    // Use this for initialization
    private void Start()
    {
        _slider = GetComponent<Slider>();
        Debug.Assert(_slider != null);

        _slider.onValueChanged.AddListener(PassInfoIntoSingleton);
    }
    
    private void PassInfoIntoSingleton(float f)
    {
        TableControlsManager.instance.SetSlider(_position, _slider.value);
    }
}
