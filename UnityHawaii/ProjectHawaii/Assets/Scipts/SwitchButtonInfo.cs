using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class SwitchButtonInfo : MonoBehaviour, IResetable
{
    [SerializeField]
    private int _position = 0;
    private UnityEngine.UI.Toggle _toggle = null;
    public AudioSource switchFlickAudioSource;

    // Use this for initialization
    private void Start()
    {
        _toggle = GetComponent<UnityEngine.UI.Toggle>();
        Debug.Assert(_toggle != null);

        _toggle?.onValueChanged.AddListener(PassInfoToSingleton);
    }

    private void PassInfoToSingleton(bool b)
    {
        TableControlsManager.Instance.SetSwitch(_position, _toggle.isOn);
    }

    public void Reset()
    {
        _toggle.isOn = false;
    }


    public void SetActiveEx(bool isSet)
    {
        transform.GetChild(0).gameObject.SetActive(!isSet);
        switchFlickAudioSource.Play();
    }
}
