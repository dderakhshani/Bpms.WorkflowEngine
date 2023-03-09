using System;
using System.Linq.Expressions;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;

namespace Bpms.WorkflowEngine
{
    public class WorkflowEngineEssentials : IWorkflowEngineEssentials
    {
        public string PerformersQuery { get; set; }

        public WorkflowEngineEssentials(string performersQuery)
        {
            this.PerformersQuery = performersQuery;
        }
    }
}