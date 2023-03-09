using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Bpms.WorkflowEngine.Infrastructure;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;

namespace Bpms.WorkflowEngine.Databases.SqlServer
{
    public interface IUnitOfWork
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class,IBaseEntity;
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}