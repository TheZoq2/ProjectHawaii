using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Messages {
    public static class MessageType {
        public static short NewClientMessage = 99;
        public static short SequenceStart = 100;
        public static short SequenceComplete = 101;
    }

    public enum DisasterType {
        Vulcano,
        Earthquake,
        Missle,
        Tornado,
        Total
    }

    public enum Component {
        Lever,
        Wheel,
        Switches,
        Scroll,
        Sliders,
        Total
    }

    public class ComponentState : MessageBase {
        public Component component;
        public int[] targets = new int [4];
    }

    public class Sequence : MessageBase {
        public int index;
        public DisasterType disaster;
        public ComponentState[] components = new ComponentState[100];
        public int timer;
    }


    public class SequenceComplete : MessageBase {
        public int index;
        public bool correct;
    }

    public class NewClientMessage : MessageBase{
        public int id;
    }
}
