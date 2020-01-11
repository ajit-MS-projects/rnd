namespace Affilinet.Business.ProductImport.Entity
{
    public class ProgramFields
    {
        public int ImportSchemaID { get; set; }
        public int ColumnOrder { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int MaxLength { get; set; }
        public string DataType { get; set; }
        public string DestField { get; set; }
        public bool ReverseOrder { get; set; }
        public bool IsProperty { get; set; }
        public bool IsNumeric { get; set; }
        public bool IsElement { get; set; }
        public bool IsAttribute { get; set; }
        public bool IsProductTag { get; set; }
        public string ParentTag { get; set; }
    }
}