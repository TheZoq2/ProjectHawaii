using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

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
        TableControlsManager.Instance.SetScrollwheel(_scrollbar.value);
        TableControlsManager.Instance.AddResetable(this);
        //return _scrollbar.value;
    }


    public void Reset()
    {
        //_scrollbar.value = 0;
        DOTween.To(() =>
            _scrollbar.value,
            value =>
        { _scrollbar.value = value; },
            0, 1);
        //StartCoroutine(UntilComplete());
    }

    private IEnumerator UntilComplete()
    {
        float old = _scrollbar.value;
        while ((_scrollbar.value =
                   Mathf.MoveTowards(old, 0, 0.1f)) - old > Mathf.Epsilon)
        {
            Debug.Log("Trying to get home.");
            old = _scrollbar.value;
            yield return null;
        }
    }
}
