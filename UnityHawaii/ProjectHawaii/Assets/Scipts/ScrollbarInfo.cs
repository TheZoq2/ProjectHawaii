using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarInfo : MonoBehaviour
{

    private Scrollbar _scrollbar = null;
    // Use this for initialization
    void Start()
    {
        _scrollbar = GetComponent<Scrollbar>();
        Debug.Assert(_scrollbar != null);

        _scrollbar?.onValueChanged.AddListener(PassInfoToSingleton);

    }

    private void PassInfoToSingleton(float v)
    {
        TableControlsManager.instance.SetScrollwheel(_scrollbar.value);
        //return _scrollbar.value;
    }
}
