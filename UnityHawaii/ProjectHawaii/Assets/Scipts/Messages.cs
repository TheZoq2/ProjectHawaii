using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Messages
{
    public static class MessageType
    {
        public static short SequenceStart = 100;
        public static short ComponentComplete = 102;
        public static short NotifyComponentComplete = 103;
    }

    public enum DisasterType
    {
        Vulcano,
        Earthquake,
        Missle,
        Tornado,
        Total
    }

    public enum Component : int
    {
        Wheel,
        Switches,
        Scroll,
        Sliders,
        Total
    }

    public class ComponentState : MessageBase, IEqualityComparer<ComponentState>
    {
        public Component component;
        public int[] targets = new int[4] { -1, -1, -1, -1 };

        public ComponentState()
        {
        }

        public ComponentState(Component component, params int[] targets)
        {
            this.component = component;
            this.targets = new[] { -1, -1, -1, -1 };
            System.Array.Copy(targets, 0, this.targets, 0, Mathf.Min(targets.Length, 3));
        }

        public bool Equals(ComponentState x, ComponentState y)
        {
            if (x == null || y == null) return false;
            if (x.component != y.component) return false;

            //return x.targets.SequenceEqual(y.targets);

            //Linq version - also works
            return !x.targets.Where((t, i) => Mathf.Abs(t - y.targets[i]) > 10).Any();

            //Works - but more understandable
            //for (int i = 0; i < x.targets.Length; i++)
            //    if (Mathf.Abs(x.targets[i] - y.targets[i]) > 10) return false;

            //return true;
        }

        public int GetHashCode(ComponentState obj)
        {
            return obj.GetHashCode();
        }

        public override string ToString()
        {
            return component.ToString() + ": " + PrintCollection(targets);
        }

        private string PrintCollection<T>(IEnumerable<T> collection)
        {
            //return collection.Aggregate("", (current, item) => current + (item.ToString() + ", "));

            string result = "";
            foreach (var item in collection)
                result += item.ToString() + ", "; // Replace this with your version of printing

            return result;
        }
    }

    public class Sequence : MessageBase
    {
        public int index;
        public DisasterType disaster;
        public ComponentState[] components = new ComponentState[100];
        public int timer;
        public int island;
        public bool shouldHandle;

        public Sequence()
        {
        }

        public Sequence(int index, DisasterType disaster, int timer, List<ComponentState> componentStates)
        {
            this.index = index;
            this.disaster = disaster;
            this.timer = timer;
            this.components = componentStates.ToArray();
        }

        public Sequence(int index, DisasterType disaster, int timer, params ComponentState[] componentStates)
        {
            this.index = index;
            this.disaster = disaster;
            this.timer = timer;
            this.components = componentStates;
        }
    }


    public class ComponentComplete : MessageBase {
    }
    public class NotifyComponentComplete : MessageBase {
        
    }
}
