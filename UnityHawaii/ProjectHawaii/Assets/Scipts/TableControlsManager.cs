using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Messages;
using UnityEngine;
// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Local

//#pragma warning disable 0414
public class TableControlsManager : MonoBehaviour
{
    private Coroutine _componentResetCoroutine;
    private Coroutine _sequencesFlushCoroutine;

    private ComponentState _lastComponent;
    private HashSet<IResetable> _resetables;

    public static TableControlsManager Instance { get; private set; }

    [SerializeField, ReadOnly]
    private Sequence _serverSequence;
    [SerializeField, ReadOnly]
    private List<ComponentState> _mySequence;

    private object _lockObject = new object();

    public delegate void SequenceComplete(Messages.SequenceComplete completeSequence);

    public event SequenceComplete OnSequenceComplete;

    private void Start()
    {
        Instance = this;
        _resetables = new HashSet<IResetable>();

        OnSequenceComplete += Received;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SupplySequence(new Sequence(0, DisasterType.Earthquake, 5,
                new ComponentState(Messages.Component.Lever, 3)));
        }
        if (Input.GetKeyDown(KeyCode.Space)) CompleteAnswerAndSend(false);
    }

    private void Received(Messages.SequenceComplete obj)
    {
        Debug.Log("Received Completed Sequence with result: " + obj.correct.ToString());
    }

    #region Answers

    private void CompleteAnswerAndSend(bool flush = true)
    {
        Messages.SequenceComplete result =
            flush ? IsSequenceCorrect_Flush() : IsSequenceCorrect();
        Debug.Log("Completed answer in time with result: " + result.correct.ToString());

        OnSequenceComplete?.Invoke(result);
    }

    private Messages.SequenceComplete TimeOut()
    {
        return new Messages.SequenceComplete(
            _serverSequence?.index ?? -1, false);
    }

    private Messages.SequenceComplete TimeOut_Flush()
    {
        Messages.SequenceComplete result = TimeOut();
        FlushLocalSequence();
        return result;
    }

    private void TimeOutAnswer(bool flush = true)
    {
        Messages.SequenceComplete result =
            flush ? TimeOut_Flush() : TimeOut();

        Debug.Log("Failed to answer in time.");
        OnSequenceComplete?.Invoke(result);
    }

    private Messages.SequenceComplete IsSequenceCorrect()
    {
        if (_mySequence == null) return new Messages.SequenceComplete(-1, false);

        if (_mySequence.Count < _serverSequence.components.Length)
        {
            Debug.Log("Local Sequence not full.");
            return new Messages.SequenceComplete(_serverSequence.index, false);
        }

        bool correct = _mySequence.SequenceEqual(_serverSequence.components);

        return new Messages.SequenceComplete(_serverSequence.index, correct);
    }

    private void FlushLocalSequence()
    {
        if (_sequencesFlushCoroutine == null)
            _sequencesFlushCoroutine = StartCoroutine(WaitForEndOfFrame(() =>
            {
                _mySequence = null;
                _serverSequence = null;
                _lastComponent = null;

                foreach (IResetable resetable in _resetables)
                    resetable.Reset();
                _resetables = new HashSet<IResetable>();

                Debug.Log("Flushed local and server sequence. (Client-side Cache)");

                _sequencesFlushCoroutine = null;
            }));
    }

    private Messages.SequenceComplete IsSequenceCorrect_Flush()
    {
        var result = IsSequenceCorrect();
        FlushLocalSequence();
        return result;
    }
    #endregion

    #region Sequence Management

    public void SupplySequence(Sequence sequence)
    {
        _serverSequence = sequence;
        _mySequence = new List<ComponentState>();

        StartCoroutine(Wait(sequence.timer, () => { TimeOutAnswer(); }));
    }
    private void Log(ComponentState loggedComponent)
    {
        if (_mySequence != null && _lastComponent != null &&
            loggedComponent != null &&
            DefendAgainstOverflow(loggedComponent)) return;

        _lastComponent = loggedComponent;
        if (_mySequence == null) return;
        _mySequence.Add(loggedComponent);
        if (_mySequence.Count >= _serverSequence.components.Length)
        {
            Debug.Log("My Sequence Full. Getting Correct Answer.");

            CompleteAnswerAndSend(false);
            //throw new NotImplementedException("My Sequence Full");
        }

        //Debug.Log(_mySequence.ToString());
        Debug.Log(PrintCollection(_mySequence));
    }

    private bool DefendAgainstOverflow(ComponentState component)
    {
        if (_componentResetCoroutine != null)
        {
            StopCoroutine(_componentResetCoroutine);
            _componentResetCoroutine = null;
        }

        if (component.component != _lastComponent.component)
        {
            return false;
        }

        _componentResetCoroutine = StartCoroutine(Wait(2, () => { _lastComponent = null; }));

        try
        {
            lock (_lockObject)
            {
                if (_mySequence.Count > 0)
                    _mySequence[_mySequence.Count - 1] = component;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            StartCoroutine(Wait(0.05f, () =>
             {
                 _mySequence[_mySequence.Count - 1] = component;
             }));
        }

        return true;
    }

    #endregion

    #region Other
    private string PrintCollection<T>(IEnumerable<T> collection)
    {
        string result = "";
        if (collection == null) return result;

        return collection.Aggregate(result, (current, item) => current + (item + ", "));
    }

    //Coroutines
    private IEnumerator Wait(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    private IEnumerator WaitForEndOfFrame(Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }

    #endregion

    #region Public Receiver Functions

    public void AddResetable(IResetable resetable)
    {
        _resetables.Add(resetable);
    }

    public void SetLever(int position)
    {
        //_leverPosition = position;

        Log(new ComponentState(Messages.Component.Lever, position));
    }

    public void SetWheel(float angle, bool radians = false)
    {
        if (radians) angle *= Mathf.Rad2Deg;
        int wheelAngle = (int)angle;

        Log(new ComponentState(Messages.Component.Wheel, wheelAngle));
    }

    public void SetScrollwheel(float scroll)
    {
        int scrollbar = (int)(scroll * 100);

        Log(new ComponentState(Messages.Component.Scroll, scrollbar));
    }

    public void SetSwitch(int position, bool switchValue = false)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Switch Position out of range (0..2).");
        //_switches[position - 1] = switchValue;

        Log(new ComponentState(Messages.Component.Switches, position, Convert.ToInt32(switchValue)));
    }

    public void SetSlider(int position, float sliderValue = 0)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Slider Position out of range (0..2).");
        if (sliderValue < 0 && sliderValue > 1)
            throw new InvalidOperationException
                ("Input Slider Value out of range (0..1).");

        //_sliders[position - 1] = (int)(sliderValue * 100);

        Log(new ComponentState(Messages.Component.Sliders, position, (int)(sliderValue * 100)));
    }

    #region old
    #endregion
    #endregion
}
