namespace InvestmentManagementReport.Model
{
    public sealed class NameValuePair
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public void Deconstruct(out string name, out string value)
        {
            name = Name;
            value = Value;
        }
    }
}