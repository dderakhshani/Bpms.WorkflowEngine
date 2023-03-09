using System;
using System.Linq.Expressions;
using Bpms.WorkflowEngine.Databases.Entities;

namespace Bpms.WorkflowEngine.Infrastructure.Interfaces
{
    public interface IWorkflowEngineEssentials
    {
        string PerformersQuery { get; set; }
    }
}