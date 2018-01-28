using System.Collections;
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

    // Use this for initialization
    void Start()
    {
        _scrollSprites = Resources.LoadAll<Sprite>("Sprites/Scrollwheel");
        _checkboxSprites = Resources.LoadAll<Sprite>("Sprites/Toggles");
        SetSequence(new Sequence
        {
            components = new ComponentState[]
            {
                new ComponentState
                {
                    component = Component.Sliders,
                    targets = new int[]
                    {
                        25,
                        50,
                        60
                    }

                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSequence(Sequence seq)
    {
        seq.components.ToList().ForEach(c =>
        {
            switch (c.component)
            {
                case Component.Lever:
                    //TODO: Not implemented yet
                    //Instantiate(Lever, transform);
                    break;
                case Component.Scroll:
                    GameObject go = Instantiate(Scroll, transform);
                    go.GetComponent<Image>().sprite = _scrollSprites[c.targets[0] / 25 - 1];
                    break;
                case Component.Sliders:
                    go = Instantiate(Slider, transform);
                    go.transform.Find("Slider1").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[0] / 100.0f * 200, 30);
                    go.transform.Find("Slider2").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[1] / 100.0f * 200, 30);
                    go.transform.Find("Slider3").gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2((float)c.targets[2] / 100.0f * 200, 30);

                    break;
                case Component.Switches:
                    go = Instantiate(Switches, transform);
                    break;
                case Component.Wheel:
                    go = Instantiate(Wheel, transform);
                    break;
            }
        });
    }
}
