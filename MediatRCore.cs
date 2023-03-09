using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using MediatR;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Bpms.WorkflowEngine.Infrastructure.Mappings;
using Task = System.Threading.Tasks.Task;

namespace Bpms.WorkflowEngine
{
    public class MediatRCore
    {
        #region StartWorkflow

        public class StartWorkflow : IRequest<RuntimeActivity>
        {
            public string WorkFlowUniqueName { get; set; }
            public int UserId { get; set; }

        }

        public class StartWorkflowHandler : IRequestHandler<StartWorkflow, RuntimeActivity>
        {
            private readonly IWorkflowDefinition _workflowDefinition;
            private readonly IWorkflowEngine _workflowEngine;

            public StartWorkflowHandler(IWorkflowDefinition workflowDefinition, IWorkflowEngine workflowEngine)
            {
                _workflowDefinition = workflowDefinition;
                _workflowEngine = workflowEngine;
            }


            public async Task<RuntimeActivity> Handle(StartWorkflow request, CancellationToken cancellationToken)
            {
                return await _workflowEngine.StartWorkflow(request.WorkFlowUniqueName, request.UserId);
            }
        }

        #endregion

        #region NextStep
        public class NextStep : IRequest
        {
            public RuntimeActivity RunningActivity { get; set; }
            public int Priority { get; set; }
            public int PrimaryKeyValue { get; set; }
            public string TaskTitle { get; set; }
            public Dictionary<string, string> ParamValues { get; set; }
        }

        public class NextStepHandler : IRequestHandler<NextStep>
        {
            private readonly IWorkflowDefinition _workflowDefinition;
            private readonly IWorkflowEngine _workflowEngine;

            public NextStepHandler(IWorkflowDefinition workflowDefinition, IWorkflowEngine workflowEngine)
            {
                _workflowDefinition = workflowDefinition;
                _workflowEngine = workflowEngine;
            }

            async Task<Unit> IRequestHandler<NextStep, Unit>.Handle(NextStep request, CancellationToken cancellationToken)
            {
                await _workflowEngine.ActivateNextActivities(request.RunningActivity, request.Priority,
                    request.PrimaryKeyValue, request.TaskTitle);
                return Unit.Value;
            }
            #endregion
        }
    }
}