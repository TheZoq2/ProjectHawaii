﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Messages;
using UnityEngine;
using UnityEngine.UI;
using Component = Messages.Component;

public class SequencePanelScript : MonoBehaviour
{

    public GameObject Lever;
    public GameObject Scroll;
    public GameObject Slider;
    public GameObject Switches;
    public GameObject Wheel;

    private Sprite[] _scrollSprites;
    private Sprite[] _checkboxSprites;

    private int _panelId;
    public GameObject WarningImage;

    // Use this for initialization
    void Awake()
    {
        _scrollSprites = Resources.LoadAll<Sprite>("Sprites/Scrollwheel");
        _checkboxSprites = Resources.LoadAll<Sprite>("Sprites/Toggles");
        //SetSequence(new Sequence
        //{
        //    components = new ComponentState[]
        //    {
        //        new ComponentState
        //        {
        //            component = Component.Lever,
        //            targets = new int[]
        //            {
        //                2
        //            }

        //        }
        //    }
        //});
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSequenceAndId(Sequence seq, int Id)
    {
        _panelId = Id;
        seq.components.ToList().ForEach(c =>
        {
            switch (c.component)
            {
                case Component.Lever:
                    Instantiate(Lever, transform);
                    break;
                case Component.Scroll:
                    GameObject go = Instantiate(Scroll, transform);
                    //go.GetComponent<Image>().sprite = _scrollSprites[c.targets[0] / 100 - 1];
                    go.transform.Find("ChildImage").GetComponent<RectTransform>().sizeDelta = new Vector2(200, (float)c.targets[0] / 100.0f * 200);
                    break;
                case Component.Sliders:
                    go = Instantiate(Slider, transform);
                    go.transform.Find("Slider1").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[0] / 100.0f * 200, 30);
                    go.transform.Find("Slider2").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[1] / 100.0f * 200, 30);
                    go.transform.Find("Slider3").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[2] / 100.0f * 200, 30);

                    break;
                case Component.Switches:
                    go = Instantiate(Switches, transform);
                    int result = c.targets[0] + c.targets[1] * 2 + c.targets[2] * 4;
                    print($"{c.targets[0]}|{c.targets[1]}|{c.targets[2]}|{result}|{_checkboxSprites.Length}");
                    go.GetComponent<Image>().sprite = _checkboxSprites[result];
                    break;
                case Component.Wheel:
                    go = Instantiate(Wheel, transform);
                    go.transform.Rotate(new Vector3(0,0,c.targets[0]));
                    break;
            }
        });
    }

    public void DecrementId()
    {
        _panelId--;
    }
}
