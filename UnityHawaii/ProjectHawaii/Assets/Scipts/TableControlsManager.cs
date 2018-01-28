using Messages;
using System;
using UnityEngine;
using Component = Messages.Component;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Local

//#pragma warning disable 0414
public class TableControlsManager : MonoBehaviour
{
    public static TableControlsManager Instance { get; private set; }
    private static SequenceWithQueue _currentSequenceToExecute;
    private static SequenceWithQueue _currentSequenceToCommunicate;
    private static bool[] _switches = new bool[3] { false, false, false };
    private static int[] _sliders = new int[3] {
        0,0,0
    };

    private void Start()
    {
        Instance = this;
        EventManager.OnSequenceItemCompleted += SequenceItemCompleted;
//
//        SupplyExecutionSequence(new Sequence
//        {
//            components = new ComponentState[4]
//            {
//                new ComponentState
//                {
//                    component = Component.Wheel,
//                    targets = new int[1] {
//                        20
//                        }
//                },
//                new ComponentState
//                {
//                    component = Component.Scroll,
//                    targets = new int[]
//                    {
//                        60
//                    }
//                },
//                new ComponentState
//                {
//                    component = Component.Switches,
//                    targets = new int[3] {
//                        1,0,1
//                }
//                },
//                new ComponentState
//                {
//                    component = Component.Sliders,
//                    targets = new int[3]
//                    {
//                        30,40,70
//                    }
//                }
//            }
//        });
    }

    // Called from Event
    public static void SequenceItemCompleted()
    {
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            return;
        }
        _currentSequenceToExecute.Components.Dequeue();
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            SequenceCompleted();
            return;
        }
        //Panel.RemoveFirstItem();
        ReadNextSequenceItem();
    }

    private static void SequenceCompleted()
    {
        print("finished");
    }

    private static void ReadNextSequenceItem()
    {
        print(_currentSequenceToExecute.Components.Peek().component);
        EventManager.SequenceItemHasChanged(_currentSequenceToExecute);
    }


    #region Public Receiver Functions

    public void SetLever(int position)
    {
        //_leverPosition = position;

        //Log(new ComponentState(Messages.Component.Lever, position));
    }

    public void SetWheel(float angle, bool radians = false)
    {
        if (radians) angle *= Mathf.Rad2Deg;
        int wheelAngle = (int)angle;

        if (wheelAngle == _currentSequenceToExecute.Components.Peek().targets[0])
            EventManager.SequenceItemCompleted();

        //Log(new ComponentState(Messages.Component.Wheel, wheelAngle));
    }

    public void SetScrollwheel(float scroll)
    {
        int scrollbar = (int)((1 - scroll) * 100);

        if (Mathf.Abs(scrollbar - _currentSequenceToExecute.Components.Peek().targets[0]) < 12)
            EventManager.SequenceItemCompleted();

        //Log(new ComponentState(Messages.Component.Scroll, scrollbar));
    }

    public void SetSwitch(int position, bool switchValue = false)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Switch Position out of range (0..2).");

        _switches[position - 1] = !switchValue;
        if(_currentSequenceToExecute.Components.Peek().component == Component.Switches)
            CheckSwitches();
    }

    private void CheckSwitches()
    {
        var target = _currentSequenceToExecute.Components.Peek();
        bool[] boolArray = new bool[3]
        {
            target.targets[0] != 0,
            target.targets[1] != 0,
            target.targets[2] != 0
        };

        print($"{_switches[0]}|{boolArray[0]}|{_switches[1]}|{boolArray[1]}|{_switches[2]}|{boolArray[2]}|");

        if (_switches[0] == boolArray[0] && _switches[1] == boolArray[1] && _switches[2] == boolArray[2])
            EventManager.SequenceItemCompleted();
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
        CheckSliders();
    }

    private void CheckSliders()
    {
        var target = _currentSequenceToExecute.Components.Peek();
        bool[] boolArray = new bool[3]
        {
            target.targets[0] != 0,
            target.targets[1] != 0,
            target.targets[2] != 0
        };
        if (Math.Abs(_sliders[0] - target.targets[0]) < 15 && Math.Abs(_sliders[1] - target.targets[1]) < 15 && Math.Abs(_sliders[2] - target.targets[2]) < 15)
            EventManager.SequenceItemCompleted();
    }

    #endregion
    // Sequence for other client
    // If this is called this means that the time ran out or that the sequence was completed and the next one has been delivered
    public static void SupplyExecutionSequence(Sequence sequence)
    {
        _currentSequenceToExecute = MapToSequenceWithQueue(sequence);
        ReadNextSequenceItem();
    }

    // Sequence for this client
    public static void SupplyCommunicationSequence(Sequence sequence)
    {
        _currentSequenceToCommunicate = MapToSequenceWithQueue(sequence);
        ResetSequencePanels();
        GenerateSequencePanels();
    }

    private static void ResetSequencePanels()
    {
        throw new NotImplementedException();
    }

    private static void GenerateSequencePanels()
    {
        throw new NotImplementedException();
    }

    // Map it to an struct with queue for ease
    private static SequenceWithQueue MapToSequenceWithQueue(Sequence sequence)
    {
        return new SequenceWithQueue(sequence.index, sequence.disaster, sequence.components, sequence.timer);
    }
}
