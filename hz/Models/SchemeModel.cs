namespace hz.Models
{
    public class SchemeModel
    {
        public Dictionary<string, string> ColumnsOuter { get; set; }
        public Dictionary<string, string> ColumnsInner { get; set; }
        public List<string> Methods { get; set; }
        public List<string> Properties { get; set; }
        public string SelectedMethod { get; set; }
        public string SelectedProperty { get; set; }
        public string QueryTemplate { get; set; }
        public string Condition { get; set; }


        public SchemeModel()
        {
            ColumnsOuter = new Dictionary<string, string>();
            ColumnsInner = new Dictionary<string, string>();
            Methods = new List<string>();
            SelectedMethod = string.Empty;
            QueryTemplate = string.Empty;
            Properties = new List<string>();
            SelectedProperty = string.Empty;
            Condition = string.Empty;
        }


        public SchemeModel(Dictionary<string, string> columnsOuter, Dictionary<string, string> columnsInner)
        {
            ColumnsOuter = columnsOuter;
            ColumnsInner = columnsInner;
            
            Methods = new List<string>();
        }
    }
}
