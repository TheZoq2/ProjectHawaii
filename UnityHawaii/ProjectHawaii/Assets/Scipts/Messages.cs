using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Messages {
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

    public class ComponentMessage : MessageBase {
        public DisasterType disaster;
        public int lengthOfComponents;
        public Component component;
        public int componentState;
        public int actionId;
    }
}
