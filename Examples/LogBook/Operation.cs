using System;
using System.Runtime.Serialization;
using LogBook.Model;

namespace LogBook
{
    [DataContract]
    public class Operation:Entity
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string VIN { get; set; }
        [DataMember]
        public DateTime? Received { get; set; }
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public DateTime? Sent { get; set; }
        [DataMember]
        public string Buyer_Name { get; set; }
        [DataMember]
        public string Buyer_Address { get; set; }
        [DataMember]
        public string Buyer_State { get; set; }
        [DataMember]
        public string Buyer_Zip { get; set; }
        [DataMember]
        public bool Discarded{ get; set; }
        [DataMember]
        public string DiscardReason { get; set; }
        public string FullAddress
        {
            get
            {
                return Buyer_Address + (String.IsNullOrEmpty(Buyer_State) ? "" : (", " + Buyer_State)) +
                    (String.IsNullOrEmpty(Buyer_Zip) ? "" : (", " + Buyer_Zip));                    
            }
        }
    }
}
