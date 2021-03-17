using System;
using System.Collections.Generic;
using System.Text;

namespace MedicationSchedule.Model
{
    public class PatientInfoData
    {
        public string PatientName { get; set; }
        public string SSN { get; set; }
        public string DOB { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
