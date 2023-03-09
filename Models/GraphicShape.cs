namespace Bpms.WorkflowEngine.Models
{
    public class GraphicShape
    {
        public int Id { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
        public int? BorderColor { get; set; }
        public int? FillColor { get; set; }
        public bool? Expanded { get; set; }
        public int? ExpandedWidth { get; set; }
        public int? ExpandedHeight { get; set; }

        public int? XCoordinate { get; set; }
        public int? YCoordinate { get; set; }

        public int ShapeType { get; set; }
        public string  Type { get; set; }
        public string Name { get; set; }

    }
}
