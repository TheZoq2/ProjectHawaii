using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Messages;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Local

//#pragma warning disable 0414
public class TableControlsManager : MonoBehaviour
{
    private static class SequenceGenerator
    {
        public static SequenceWithQueue GenerateSequence()
        {
            int length = UnityEngine.Random.Range(4, 10);
            List<ComponentState> components = new List<ComponentState>();
            for (int i = 0; i < length; i++)
                components.Add(GenerateComponentState());

            var queue = new SequenceWithQueue(
                UnityEngine.Random.Range(0, 1000000),
                (DisasterType)UnityEngine.Random.Range(0, (int)DisasterType.Total),
                components.ToArray(),
                GenerateTimer(5, 10));
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

    struct SequenceWithQueue
    {
        public int index;
        public DisasterType disaster;
        public Queue<ComponentState> Components;
        public int timer;

        public SequenceWithQueue(int index, DisasterType disaster, ComponentState[] components, int timer)
        {
            this.index = index;
            this.disaster = disaster;
            this.Components = new Queue<ComponentState>(components);
            this.timer = timer;
        }

        public override string ToString()
        {
            return base.ToString() + $": index({index}), disaster({disaster}), timer({timer})";
        }
    }


    public static TableControlsManager Instance { get; private set; }
    private static SequenceWithQueue _currentSequenceToExecute;
    private static SequenceWithQueue _currentSequenceToCommunicate;


    private void Start()
    {
        Instance = this;
        EventManager.OnSequenceItemCompleted += SequenceItemCompleted;
    }

    // Called from Event
    public static void SequenceItemCompleted()
    {
        _currentSequenceToExecute.Components.Dequeue();
        //Panel.RemoveFirstItem();
        ReadNextSequenceItem();
    }

    private static void ReadNextSequenceItem()
    {
        EventManager.SequenceItemHasChanged(_currentSequenceToCommunicate.Components.Peek().component);
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

        //Log(new ComponentState(Messages.Component.Wheel, wheelAngle));
    }

    public void SetScrollwheel(float scroll)
    {
        int scrollbar = (int)(scroll * 100);

        //Log(new ComponentState(Messages.Component.Scroll, scrollbar));
    }

    public void SetSwitch(int position, bool switchValue = false)
    {
        if (position < 1 && position > 3)
            throw new InvalidOperationException
                ("Input Switch Position out of range (0..2).");
        //_switches[position - 1] = switchValue;

        //Log(new ComponentState(Messages.Component.Switches, position, Convert.ToInt32(switchValue)));
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

        // Log(new ComponentState(Messages.Component.Sliders, position, (int)(sliderValue * 100)));
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
        //Debug.Log(_currentSequenceToCommunicate);
        //ResetSequencePanels();
        var list = GenerateSequencePanels(_currentSequenceToCommunicate);
        //foreach (var thing in list)
        //    Debug.Log(thing);
    }

    private static void ResetSequencePanels()
    {
        throw new NotImplementedException();
    }

    //Gets an array with (an amount specified by howmanyothers of) Sequences 
    private static SequenceWithQueue[] GenerateSequencePanels(
        SequenceWithQueue originalSequence, int howManyOthers = 2)
    {
        //Debug.Log(howManyOthers);
        //throw new NotImplementedException();
        List<SequenceWithQueue> otherSequences = new List<SequenceWithQueue>() { originalSequence };

        //Debug.Log(howManyOthers);
        for (int i = 0; i < howManyOthers; i++)
            otherSequences.Add(GetRandomDifferentSequence(otherSequences.ToArray()));

        otherSequences.Remove(originalSequence);

        return otherSequences.ToArray();
    }

    //Gets a random sequence different from the (variable) list of provided other sequences
    //different by (currently): disaster type
    private static SequenceWithQueue GetRandomDifferentSequence(params SequenceWithQueue[] originalSequences)
    {
        List<DisasterType> disasterTypeExclusions = new List<DisasterType>() { DisasterType.Total };
        foreach (SequenceWithQueue originalSequence in originalSequences)
            disasterTypeExclusions.Add(originalSequence.disaster);

        SequenceWithQueue swq = new SequenceWithQueue(
            SequenceGenerator.GenerateIndex(500, 1000),
            ExcludeDisasters(disasterTypeExclusions.ToArray()),
            SequenceGenerator.GenerateComponentStatesArray(3),
            SequenceGenerator.GenerateTimer(500, 1000));

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
