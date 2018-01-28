using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SliderInfo : MonoBehaviour, IResetable
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
        TableControlsManager.Instance.SetSlider(_position, _slider.value);
        TableControlsManager.Instance.AddResetable(this);
    }

    public void Reset()
    {
        //StartCoroutine(UntilComplete());

        DOTween.To(() =>
                _slider.value,
            value =>
            { _slider.value = value; },
            0, 1);
    }

    private IEnumerator UntilComplete()
    {
        while (true)
        {
            float old = _slider.value;
            if ((_slider.value =
                    Mathf.MoveTowards(old, 0, 0.1f)) - old < float.Epsilon)
                yield return null;
        }
    }
}
