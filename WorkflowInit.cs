using System;
using Microsoft.EntityFrameworkCore;
using Bpms.WorkflowEngine.Databases.SqlServer.Context;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;

namespace Bpms.WorkflowEngine
{
    public class WorkflowInit : IWorkflowInit
    {
        private readonly IBpmsUnitOfWork _bpmsUnitOfWork;
        public WorkflowInit(IBpmsUnitOfWork bpmsUnitOfWork)
        {
            _bpmsUnitOfWork = bpmsUnitOfWork;
            try
            {
                bpmsUnitOfWork.DbContext().Database.Migrate();
            }
            catch(Exception e)
            {
                throw;
            }

            bpmsUnitOfWork.DbContext().Database.EnsureCreated();
        }
    }
}