namespace Bpms.WorkflowEngine.Enums
{
    internal enum ActivityTypes
    {
        Activity = 1, UserActivity = 2, ServiceActivity = 3,
        SendActivity = 4, ScriptActivity = 5, ManualActivity = 6, BussinessActivity = 7,

        StartEvent = 21, EndEvent = 22, IntermmediateEvent = 23,

        NormalGateway = 30, ParallelGateWay = 31, InclusiveGateWay = 32, EventBasedGateWay = 33,
        ExclusiveGateWay = 34, ExclusiveEventBasedGateWay = 35, ParallelEventBasedGateWay = 36, ComplexGateWay = 37,

    }
}