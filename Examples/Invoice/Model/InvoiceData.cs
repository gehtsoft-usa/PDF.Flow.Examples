using System.Runtime.Serialization;

namespace Gehtsoft.PDFFlow.Invoice.Model
{
    [DataContract]
    public class InvoiceData
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
