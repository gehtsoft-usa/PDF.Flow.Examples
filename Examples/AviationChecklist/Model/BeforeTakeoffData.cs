using System;
using System.Collections.Generic;
using System.Text;

namespace AviationChecklist.Model
{
    class BeforeTakeOffData
    {
        public string Name { get; set; }
        public string ParkingBrake { get; set; }
        public string FlightInstruments { get; set; }
        public string EngineInstruments { get; set; }
        public string TakeOffData { get; set; }
        public string NavEquipment { get; set; }
        public string LandingLights { get; set; }
        public string StrobeLight { get; set; }
        public string PitotHeat { get; set; }
        public string DeIce { get; set; }
        public string Transponder { get; set; }
    }
}
