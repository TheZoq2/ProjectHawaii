using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Component = Messages.Component;

public class ScrollbarInfo : MonoBehaviour, IResetable
{

    private Scrollbar _scrollbar = null;

    private SequenceWithQueue _currentSequenceToExecute;
    private const Component CurrentComponent = Component.Scroll;
    // Use this for initialization
    void Awake()
    {
        _scrollbar = GetComponent<Scrollbar>();
        Debug.Assert(_scrollbar != null);

        _scrollbar?.onValueChanged.AddListener(PassInfoToSingleton);

        EventManager.OnSequenceItemChanged += SequenceItemHasChanged;
    }

    private void SequenceItemHasChanged(SequenceWithQueue s)
    {
        _currentSequenceToExecute = s.Components.Peek().component == CurrentComponent ? s : null;
    }

    private void PassInfoToSingleton(float v)
    {
        if(_currentSequenceToExecute != null)
            TableControlsManager.Instance.SetScrollwheel(_scrollbar.value);
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
