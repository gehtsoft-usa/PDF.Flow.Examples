using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InvestmentManagementReport.Model
{
    public sealed class GreenTable
    {
        public List<GreenTableColumn> Columns { get; set; }
        public List<GreenTableDataItem> Data { get; set; }
    }

    public sealed class GreenTableColumn
    {
        public string Title { get; set; }
        public int ColSpan { get; set; }
        public bool IsRightAligned { get; set; }
        public int ColumnIndex { get; set; }
    }

    public sealed class GreenTableDataItem
    {
        public bool IsStart { get; set; }
        public bool IsSubItem { get; set; }
        public bool IsEnd { get; set; }
        
        [JsonExtensionData]
        public Dictionary<string, JsonElement> Data { get; set; }
    }
}