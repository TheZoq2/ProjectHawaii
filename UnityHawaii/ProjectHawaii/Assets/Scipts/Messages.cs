using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Messages {
    public static class MessageType {
        public static short SequenceStart = 100;
        public static short SequenceComplete = 101;
    }

    public enum DisasterType {
        Vulcano,
        Earthquake,
        Missle,
        Tornado
    }

    public enum Component {
        Lever,
        Wheel,
        Switches,
        Scroll,
        Sliders
    }

    public class ComponentState : MessageBase {
        public Component component;
        //public ArrayList targets;
    }

    public class Sequence : MessageBase {
        public int index;
        public DisasterType disaster;
        //public List<ComponentState> components;
        public int timer;
    }


    public class SequenceComplete : MessageBase {
        public int index;
        public bool correct;
    }
}
