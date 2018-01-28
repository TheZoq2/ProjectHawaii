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
    }


    public static TableControlsManager Instance { get; private set; }
    private static SequenceWithQueue _currentSequenceToExecute;
    private static SequenceWithQueue _currentSequenceToCommunicate;

    public delegate void SequenceComplete(Messages.SequenceComplete completeSequence);
    

    private void Start()
    {
        Instance = this;
        EventManager.OnSequenceItemCompleted += SequenceItemCompleted;
        ReadNextSequenceItem();
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
