using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0414
public class TableControlsManager : MonoBehaviour
{
    //Always receive a sequence of 2 + n * 3 (where n = Sequence length) integers: 
    //First Position: Disaster - Volcano(1), Earthquake (2), Missle (3), Tornado(4)
    //Second Position: Sequence/Symbol Length - integer (around 3 to 6 usually)

    //Every 3 define a symbol:
    //First Position: Component
    //Second Position: depending on the component - it's either a subcomponent or a value
    //Third Position: value (for subcomponent)

    //Component - Component Number Representation 
    //(Possible Sub Component Number Representation) - Possible Values
    //Lever -  1 - [0],1,2,3,4
    //Wheel - 2 - 0..360
    //Switches - 3 (1..3) - On/Off (True/False - 1/0)
    //Scrollbar - 4 - 0..100
    //Sliders - 5 (1..3) - 0..100

    private enum Disaster : int { Volcano = 1, Earthquake = 2, Missle = 3, Tornado = 4 }
    private enum Component : int { Lever = 1, Wheel = 2, Switches = 3, Scrollbar = 4, Sliders = 5 }

    private Component _lastComponent = 0;
    private int _lastSubComponent = 0; //if any
    private HashSet<IResetable> _resetables = null;

    //private static TableControlsManager _instance = null;
    //public static TableControlsManager instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new TableControlsManager();
    //            //_instance._mySequence = new List<int>();
    //            //_instance._serverSequence = new List<int> { 3, 3, 3, 3, 3 };
    //        }
    //        return _instance;
    //    }
    //}

    private static TableControlsManager _instance = null;
    public static TableControlsManager instance
    {
        get { return _instance; }
    }

    [SerializeField, ReadOnly]
    private List<int> _serverSequence = null;
    [SerializeField, ReadOnly]
    private List<int> _mySequence = null;

    //Sequence Cache
    private int _sequenceLength = -1;

    //Cache
    private int _leverPosition = -1;
    private int _wheelAngle = -1;
    private bool[] _switches = new bool[3] { false, false, false };
    private int _scrollbar = -1;
    private int[] _sliders = new int[3] { -1, -1, -1 };

    private void Start()
    {
        _instance = this;
        _resetables = new HashSet<IResetable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) FlushLocalSequence();
    }

    #region Sequence Management
    public string PrintCollection<T>(IEnumerable<T> col)
    {
        string result = "";
        foreach (var item in col)
            result += item.ToString() + ", "; // Replace this with your version of printing

        return result;
    }

    public bool SequenceIsCorrect()
    {
        if (_mySequence.Count < _serverSequence.Count - 2)
        {
            Debug.Log("Local Sequence not full.");
            return false;
        }

        List<int> localSequence = new List<int>(_mySequence);

        if (_mySequence?.Count != 0)
        {
            localSequence.Insert(0, _serverSequence[1]);
            localSequence.Insert(0, _serverSequence[0]);
        }

        return localSequence.SequenceEqual(_serverSequence);
    }

    public void FlushLocalSequence()
    {
        _mySequence = new List<int>();
        _serverSequence = new List<int>();

        foreach (IResetable resetable in _resetables)
            resetable.Reset();
        _resetables = new HashSet<IResetable>();

        Debug.Log("Flushed local and server sequence. (Client-side Cache)");
    }

    public bool GetIfSequenceIsCorrectAndFlush()
    {
        bool result = SequenceIsCorrect();
        FlushLocalSequence();
        return result;
    }

    public void SupplySequence(List<int> sequence)
    {
        _serverSequence = sequence;
        _mySequence = new List<int>();

        _sequenceLength = sequence.Count;
        //_sequenceCounter = 0;
    }

    public void SupplySequence(int[] sequence)
    {
        _serverSequence = new List<int>(sequence);
        _mySequence = new List<int>();

        _sequenceLength = sequence.Length;
        //_sequenceCounter = 0;
    }

    public void Log(params int[] list)
    {
        Component component = (Component)list[0];
        if (DefendAgainstOverflow(list, component)) return;

        _lastComponent = component;
        _mySequence.AddRange(list);
        if (_mySequence.Count >= _serverSequence.Count)
        {
            //Debug.Log("My Sequence Full");
            throw new NotImplementedException("My Sequence Full");
        }

        //Debug.Log(_mySequence.ToString());
        Debug.Log(PrintCollection(_mySequence));
    }

    private IEnumerator Wait(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    private bool DefendAgainstOverflow(int[] list, Component component)
    {
        StopAllCoroutines();

        if (component != _lastComponent)
        {
            return false;
        }
        
        StartCoroutine(Wait(2, () => { _lastComponent = 0; }));

        switch (component)
        {
            case Component.Lever:
            case Component.Scrollbar:
            case Component.Wheel:
                {
                    _mySequence[_mySequence.Count - 1] = list[1];
                    Debug.Log(PrintCollection(_mySequence));
                    return true;
                }
            case Component.Switches:
            case Component.Sliders:
                {
                    if (_lastSubComponent == list[1])
                    {
                        _mySequence[_mySequence.Count - 1] = list[2];

                        Debug.Log(PrintCollection(_mySequence));
                        return true;
                    }

                    _lastSubComponent = list[1];
                    break;
                }
            default:
                {
                    Debug.Log("Unexpected component type: " + list[0]);
                    break;
                }
        }


        return false;
    }

    #endregion

    #region Public Receiver Functions

    public void AddResetable(IResetable resetable)
    {
        _resetables.Add(resetable);
    }

    public void SetLever(int position)
    {
        _leverPosition = position;

        Log((int)Component.Lever, position);
    }

    public void SetWheel(float angle, bool radians = false)
    {
        if (radians) angle *= Mathf.Rad2Deg;
        _wheelAngle = (int)angle;

        Log((int)Component.Wheel, _wheelAngle);
    }

    public void SetScrollwheel(float scroll)
    {
        _scrollbar = (int)(scroll * 100);

        Log((int)Component.Scrollbar, _scrollbar);
    }

    public void SetSwitch(int position, bool switchValue = false)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Switch Position out of range (0..2).");
        _switches[position - 1] = switchValue;

        Log((int)Component.Switches, position, Convert.ToInt32(switchValue));
    }

    public void SetSlider(int position, float sliderValue = 0)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Slider Position out of range (0..2).");
        if (sliderValue < 0 && sliderValue > 1)
            throw new InvalidOperationException
                ("Input Slider Value out of range (0..1).");

        _sliders[position - 1] = (int)(sliderValue * 100);

        Log((int)Component.Sliders, position, (int)(sliderValue * 100));
    }

    #region old
    //public void SetSwitchesInfo(bool[] switchInfo)
    //{
    //    bool[] info = new bool[3] { false, false, false };
    //    Array.Copy(switchInfo, info, 3);

    //    _switches = info;
    //    //bool[] info = new bool[3](switchInfo);
    //}

    //public void SetSwitchesInfo(
    //    bool switch1 = false, 
    //    bool switch2 = false,
    //    bool switch3 = false)
    //{
    //    _switches[0] = switch1;
    //    _switches[1] = switch2;
    //    _switches[2] = switch3;
    //}

    //public void SetSliders(int[] slidersInfo)
    //{
    //    int[] info = new int[3] { -1, -1, -1 };
    //    Array.Copy(slidersInfo, info, 3);

    //    _sliders = info;
    //}

    //public void SetSliders(
    //    int slider1 = -1,
    //    int slider2 = -1,
    //    int slider3 = -1)
    //{
    //    _sliders[0] = slider1;
    //    _sliders[1] = slider2;
    //    _sliders[2] = slider3;
    //}
    #endregion
    #endregion
}
