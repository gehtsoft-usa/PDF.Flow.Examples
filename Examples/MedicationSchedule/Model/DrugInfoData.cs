using System;
using System.Collections.Generic;
using System.Text;

namespace MedicationSchedule.Model
{
    public class DrugInfoData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Dosage { get; set; }
        public List<string> Time { get; set; }
        public List<bool> TakeDrug { get; set; }
    }
}
