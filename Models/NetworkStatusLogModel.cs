namespace Bpms.WorkflowEngine.Models
{
   public class NetworkStatusLogModel
    {

        public string MacAddress { get; set; }
        public int? StatusNetwork { get; set; }
        public string IpAddress { get; set; }
        public int? SignalStrength { get; set; }
        public int? LinkSpeed { get; set; }
        public string ClientDateTime { get; set; }
        public string VersionApp { get; set; }
        public string DeviceMacAddress { get; set; }


    }
}
