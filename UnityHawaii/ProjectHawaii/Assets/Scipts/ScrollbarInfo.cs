using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollbarInfo : MonoBehaviour, IResetable
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
        TableControlsManager.instance.AddResetable(this);
        //return _scrollbar.value;
    }


    public void Reset()
    {
        //_scrollbar.value = 0;
        StartCoroutine(UntilComplete());
    }

    private IEnumerator UntilComplete()
    {
        while (true)
        {
            float old = _scrollbar.value;
            if ((_scrollbar.value =
                    Mathf.MoveTowards(old, 0, 0.1f)) - old > float.Epsilon)
                yield return null;
        }
    }
}
