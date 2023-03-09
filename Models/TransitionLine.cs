namespace Bpms.WorkflowEngine.Models
{
    public class TransitionLine
    {
        public int TransitionId { get; set; }
        public int? BorderColor { get; set; }
        public int XSource { get; set; }
        public int YSource { get; set; }
        public int SourceCoordinateId { get; set; }
        public int XDestination { get; set; }
        public int YDestination { get; set; }
        public int DestinationCoordinateId { get; set; }
        public bool IsFinalLineForTran { get; set; }
        public bool IsFirstLineForTran { get; set; }
        public string Direct { get; set; }
        public string Name { get; set; }


    }
}
