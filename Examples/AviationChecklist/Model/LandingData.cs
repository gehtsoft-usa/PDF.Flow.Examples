using System;
using System.Collections.Generic;
using System.Text;

namespace AviationChecklist.Model
{
    class LandingData
    {
        public string Name { get; set; }
        public string LandingGear { get; set; }
        public string Autopilot { get; set; }
        public string GoAroundAltitude { get; set; }
        public string AutoThrust { get; set; }
        public string LandingMemo { get; set; }
        public string LandingSpeed { get; set; }
        public string AfterTouchDown { get; set; }
        public string Spoilers { get; set; }
        public string Brakes { get; set; }
        public string At60KIAS { get; set; }
    }
}
