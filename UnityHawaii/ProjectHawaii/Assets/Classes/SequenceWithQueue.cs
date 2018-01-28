using Messages;
using System.Collections.Generic;

public class SequenceWithQueue
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
