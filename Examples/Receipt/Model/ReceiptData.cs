using System.Runtime.Serialization;

namespace Receipt.Model
{
    [DataContract]
    public class ReceiptData
    {
            [DataMember]
            public string Field { get; set; }
            [DataMember]
            public string Value { get; set; }

            public override string ToString()
            {
                return string.Concat($"{Field}\n{Value}");
            }        
    }
}
