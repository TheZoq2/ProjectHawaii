using Messages;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Component = Messages.Component;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Local

//#pragma warning disable 0414
//public struct SequenceWithQueue
//{
//    public int index;
//    public DisasterType disaster;
//    public Queue<ComponentState> Components;
//    public int timer;

//    public SequenceWithQueue(int index, DisasterType disaster, ComponentState[] components, int timer)
//    {
//        this.index = index;
//        this.disaster = disaster;
//        this.Components = new Queue<ComponentState>(components);
//        this.timer = timer;
//    }

//    public override string ToString()
//    {
//        return base.ToString() + $": index({index}), disaster({disaster}), timer({timer})";
//    }
//}

public class TableControlsManager : MonoBehaviour
{
    public static class SequenceGenerator
    {
        public static Sequence GenerateSequence()
        {
            int length = UnityEngine.Random.Range(4, 10);
            List<ComponentState> components = new List<ComponentState>();
            for (int i = 0; i < length; i++)
                components.Add(GenerateComponentState());

            var queue = new Sequence(
                UnityEngine.Random.Range(0, 1000000),
                (DisasterType)UnityEngine.Random.Range(0, (int)DisasterType.Total),
                GenerateTimer(5, 10),
                components.ToArray());
            return queue;
        }

        public static ComponentState[] GenerateComponentStatesArray(int length = 3)
        {
            List<ComponentState> componentStates = new List<ComponentState>();
            for (int i = 0; i < 3; i++)
                componentStates.Add(GenerateComponentState());

            return componentStates.ToArray();
        }

        public static ComponentState GenerateComponentState()
        {
            Messages.Component component = (Messages.Component)UnityEngine.Random.Range(
                0, (int)Messages.Component.Total);

            List<int> targets = new List<int>();

            switch (component)
            {
                case Messages.Component.Wheel:
                    targets = GetWheelTargets();
                    break;
                case Messages.Component.Switches:
                    targets = GetSwitchTargets();
                    break;
                case Messages.Component.Scroll:
                    targets = GetScrollTargets();
                    break;
                case Messages.Component.Sliders:
                    targets = GetSliderTargets();
                    break;
                default:
                    break;
            }

            ComponentState result = new ComponentState
            {
                targets = targets.ToArray(),
                component = component
            };
            return result;
        }

        public static List<int> GetWheelTargets(int min = 0, int max = 361)
        {
            var result = GetFourIntArray();
            result[0] = UnityEngine.Random.Range(min, max);
            return result;
        }

        //Max is exclusive
        public static List<int> GetSwitchTargets(int min = 0, int max = 3)
        {
            return new List<int>(){
            UnityEngine.Random.Range(min,max),
            UnityEngine.Random.Range(min,max),
            UnityEngine.Random.Range(min,max),
            UnityEngine.Random.Range(min,max)
        };
        }

        //Max is exclusive
        public static List<int> GetScrollTargets(int min = 0, int max = 101)
        {
            var result = GetFourIntArray();
            result[0] = UnityEngine.Random.Range(min, max);
            return result;
        }

        //Max is exclusive
        public static List<int> GetSliderTargets(int min = 0, int max = 101)
        {
            var result = GetFourIntArray();
            result[0] = UnityEngine.Random.Range(min, max);
            result[1] = UnityEngine.Random.Range(min, max);
            result[2] = UnityEngine.Random.Range(min, max);
            return result;
        }

        public static List<int> GetFourIntArray()
        {
            return new List<int> { 0, 0, 0, 0 };
        }

        public static int GenerateTimer(int min = 5, int max = 10)
        {
            return UnityEngine.Random.Range(5, 10);
        }

        public static int GenerateIndex(int min = 5, int max = 10)
        {
            return UnityEngine.Random.Range(5, 10);
        }
    }
    
    public static TableControlsManager Instance { get; private set; }
    private static SequenceWithQueue _currentSequenceToExecute;
    private static SequenceWithQueue _currentSequenceToCommunicate;

    private static bool[] _switches = new bool[3] { false, false, false };
    private static int[] _sliders = new int[3] {
        0,0,0
    };

    private static List<SequencePanelScript> _cleaners = null;

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
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            return;
        }
        if (wheelAngle == _currentSequenceToExecute.Components.Peek().targets[0])
            EventManager.SequenceItemCompleted();

        //Log(new ComponentState(Messages.Component.Wheel, wheelAngle));
    }

    public void SetScrollwheel(float scroll)
    {
        int scrollbar = (int)((1 - scroll) * 100);
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            return;
        }
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
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            return;
        }
        if (_currentSequenceToExecute.Components.Peek().component == Component.Switches)
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

        
        // Log(new ComponentState(Messages.Component.Sliders, position, (int)(sliderValue * 100)));

        _sliders[position - 1] = (int)(sliderValue * 100);
        CheckSliders();
    }

    private void CheckSliders()
    {
        if (_currentSequenceToExecute.Components.Count == 0)
        {
            return;
        }
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
    //Return fake sequences
    public static void SupplyCommunicationSequence(
        Sequence sequence, GameObject holder,
        GameObject sequencePanelPrefab, GameObject warningImagePrefab,
        Dictionary<DisasterType, Sprite> spriteDictionary)
    {
        _currentSequenceToCommunicate = MapToSequenceWithQueue(sequence);
        //Debug.Log(_currentSequenceToCommunicate);
        ResetSequencePanels();
        var fakeSequences = GenerateSequencePanels(sequence);

        _cleaners.Add(DrawSequence(sequence, holder, sequencePanelPrefab,
            warningImagePrefab, spriteDictionary, true)
        );

        foreach (Sequence fakeSequence in fakeSequences)
            _cleaners.Add(DrawSequence(fakeSequence, holder, sequencePanelPrefab,
                warningImagePrefab, spriteDictionary, true));
    }

    private static void ResetSequencePanels()
    {
        //throw new NotImplementedException();
        foreach (SequencePanelScript cleaner in _cleaners)
            while (cleaner != null) cleaner.PopPanel(true);
        _cleaners = null;
    }

    private static SequencePanelScript DrawSequence(
        Sequence sequence, GameObject holder, 
        GameObject sequencePanelPrefab, GameObject warningImagePrefab,
        Dictionary<DisasterType, Sprite> spriteDictionary, 
        bool setTest = false)
    {
        GameObject panel = Instantiate(sequencePanelPrefab, holder.transform);
        var script = panel.GetComponent<SequencePanelScript>();

        script.SetSequenceAndId(sequence, Statics.Panels.Count);
        //Testing
        //if (setTest) sps = script;
        //Testing

        holder = GameObject.Find("WarningPanel");
        GameObject warningImage = Instantiate(warningImagePrefab, holder.transform);

        warningImage.GetComponent<Image>().sprite = spriteDictionary[sequence.disaster];
        warningImage.GetComponent<Image>().color = new Color32(0, 0, 0, 255);

        script.WarningImage = warningImage;
        Statics.Panels.Add(panel);

        return script;
    }

    //Gets an array with (an amount specified by howmanyothers of) Sequences 
    private static Sequence[] GenerateSequencePanels(
        Sequence originalSequence, int howManyOthers = 2)
    {
        //Debug.Log(howManyOthers);
        //throw new NotImplementedException();
        List<Sequence> otherSequences = new List<Sequence>() { originalSequence };

        //Debug.Log(howManyOthers);
        for (int i = 0; i < howManyOthers; i++)
            otherSequences.Add(GetRandomDifferentSequence(otherSequences.ToArray()));

        otherSequences.Remove(originalSequence);

        return otherSequences.ToArray();
    }

    //Gets a random sequence different from the (variable) list of provided other sequences
    //different by (currently): disaster type
    private static Sequence GetRandomDifferentSequence(params Sequence[] originalSequences)
    {
        List<DisasterType> disasterTypeExclusions = new List<DisasterType>() { DisasterType.Total };
        foreach (Sequence originalSequence in originalSequences)
            disasterTypeExclusions.Add(originalSequence.disaster);

        Sequence swq = new Sequence(
            SequenceGenerator.GenerateIndex(500, 1000),
            ExcludeDisasters(disasterTypeExclusions.ToArray()),
            SequenceGenerator.GenerateTimer(500, 1000),
            SequenceGenerator.GenerateComponentStatesArray(3));

        //Debug.Log(swq);

        return swq;
    }

    private static DisasterType ExcludeDisasters(params DisasterType[] exclusions)
    {
        List<DisasterType> disasters = new List<DisasterType>()
            { DisasterType.Earthquake, DisasterType.Missle,
                DisasterType.Tornado, DisasterType.Total, DisasterType.Vulcano};

        foreach (DisasterType exclusion in exclusions)
            disasters.Remove(exclusion);

        return disasters[UnityEngine.Random.Range(0, disasters.Count)];
    }

    // Map it to an struct with queue for ease
    private static SequenceWithQueue MapToSequenceWithQueue(Sequence sequence)
    {
        return new SequenceWithQueue(sequence.index, sequence.disaster, sequence.components, sequence.timer);
    }
}
