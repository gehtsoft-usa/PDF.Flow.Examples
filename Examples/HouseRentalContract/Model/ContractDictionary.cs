using System.Runtime.Serialization;

namespace HouseRentalContract.Model
{
    [DataContract]
    public class ContractDictionary
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
