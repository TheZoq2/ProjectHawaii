using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using Component = Messages.Component;

public class SwitchButtonInfo : MonoBehaviour
{
    [SerializeField]
    private int _position = 0;
    private UnityEngine.UI.Toggle _toggle = null;
    public AudioSource switchFlickAudioSource;

    private SequenceWithQueue _currentSequenceToExecute;
    private const Component CurrentComponent = Component.Switches;

    // Use this for initialization
    private void Start()
    {
        _toggle = GetComponent<UnityEngine.UI.Toggle>();
        Debug.Assert(_toggle != null);

        _toggle?.onValueChanged.AddListener(PassInfoToSingleton);
        EventManager.OnSequenceItemChanged += SequenceItemHasChanged;
    }

    private void SequenceItemHasChanged(SequenceWithQueue s)
    {
        _currentSequenceToExecute = s.Components.Peek().component == CurrentComponent ? s : null;
    }
    private void PassInfoToSingleton(bool b)
    {
        TableControlsManager.Instance.SetSwitch(_position, _toggle.isOn);
    }

    public void SetActiveEx(bool isSet)
    {
        transform.GetChild(0).gameObject.SetActive(!isSet);
        //switchFlickAudioSource.Play();
    }
}
