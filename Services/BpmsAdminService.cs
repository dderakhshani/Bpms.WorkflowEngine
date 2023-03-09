using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Databases.SqlServer;
using Bpms.WorkflowEngine.Infrastructure.Interfaces;
using Bpms.WorkflowEngine.Infrastructure.Mappings;

namespace Bpms.WorkflowEngine.Services
{
    public interface IBpmsAdminService
    {
        Task<Workflow> CreateWorkflow(BpmsAdminService.CreateWorkflowModel model);
        Task<Workflow> GetWorkflowWithDetails(int id);
        Task<List<Workflow>> GeAllWorkflows();
        Task<Performer> AddPerformer(Performer performer);


    }

    public class BpmsAdminService : IBpmsAdminService
    {
        private readonly IWorkflowDefinition _workflowDefinition;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public BpmsAdminService(IWorkflowDefinition workflowDefinition, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _workflowDefinition = workflowDefinition;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public class CreateWorkflowModel : IMapFrom<CreateWorkflowModel>
        {
            public IFormFile XpdlFile { get; set; }
            public Guid Guid { get; set; } = default!;
            public string Title { get; set; } = default!;
            public int? CreatedBy { get; set; }
            public int? DocumentId { get; set; }
            public string? Description { get; set; }
            public string UniqueName { get; set; }
            public string WorkflowName { get; set; }
            public bool IsActive { get; set; }
            public int Version { get; set; }
            public void Mapping(Profile profile)
            {
                profile.CreateMap<CreateWorkflowModel, Workflow>()
                    .IgnoreAllNonExisting();
            }
        }
        public class CreateWorkflowModelValidator : AbstractValidator<CreateWorkflowModel>
        {
            public CreateWorkflowModelValidator()
            {
                RuleFor(x => x.Title).Must(x => x?.Length > 5).WithMessage("تعداد کاراکتر باید بیشتر از 5 عدد باشد");
                RuleFor(x => x.UniqueName).NotNull();
                RuleFor(x => x.UniqueName).NotEmpty();
                RuleFor(x => x.WorkflowName).NotNull();
                RuleFor(x => x.WorkflowName).NotEmpty();
                RuleFor(x => x.XpdlFile).NotNull();
                RuleFor(x => x.XpdlFile).NotEmpty();
                RuleFor(x => x.XpdlFile).Must(x => x != null && !string.IsNullOrEmpty(x.FileName) && Path.GetExtension(x.FileName ?? "").Equals(".xpdl", StringComparison.OrdinalIgnoreCase));
                RuleFor(x => x.Version).NotEmpty();
            }
        }
        public async Task<Workflow> CreateWorkflow(CreateWorkflowModel model)
        {
            var workflow = _mapper.Map<Workflow>(model);
            workflow.CreateTime = DateTime.Now;

            if (model.XpdlFile.Length <= 0)
            {
            }

            var reletivePath = Path.Combine("", $"{model.XpdlFile.FileName}");

            var dir = new System.IO.FileInfo(reletivePath);
            dir?.Directory?.Create();

            await using (var myFile = System.IO.File.Create(reletivePath))
            {
                model.XpdlFile.CopyToAsync(myFile).GetAwaiter().GetResult();
            }

            var xdoc = XDocument.Load(reletivePath);

            workflow.Guid = new Guid(xdoc.Root.Attribute("Id")?.Value);
            workflow.Title ??= xdoc.Root.Attribute("Name")?.Value;


            var previousVersionWorkflows = await _unitOfWork.Set<Workflow>().Where(x => x.Guid == workflow.Guid).ToListAsync();
            if (previousVersionWorkflows.Any(x => x.Version == workflow.Version))
            {
                workflow.Version = previousVersionWorkflows.Max(x => x.Version) + 1;
            }

            if (workflow.IsActive)
            {
                previousVersionWorkflows.ForEach(x => x.IsActive = false);
            }

            await _unitOfWork.SaveAsync(CancellationToken.None);


            return await _workflowDefinition.ImportXpdl(reletivePath, workflow);
        }


        public async Task<Workflow> GetWorkflowWithDetails(int id)
        {
           return await _unitOfWork.Set<Workflow>()
               .Include(x => x.Processes).ThenInclude(x => x.Activities).ThenInclude(x => x.Shape)
               .Include(x => x.Pools).ThenInclude(x => x.Shape)
               .Include(x => x.Processes).ThenInclude(x => x.Transitions).ThenInclude(x => x.TransitionShapes).ThenInclude(x => x.Shape)
               .Include(x => x.Pools).ThenInclude(x => x.Lanes).ThenInclude(x => x.Shape)
                .FirstAsync(x => x.Id == id);
        }

        public async Task<List<Workflow>> GeAllWorkflows()
        {
            return await _unitOfWork.Set<Workflow>().ToListAsync();
        }


        public async Task<Performer> AddPerformer(Performer performer)
        {
            var childs = performer.PerformerConditions;
            performer.PerformerConditions = null;
            var addedPerformer = _unitOfWork.Set<Performer>().Add(performer);
            foreach (var performerPerformerCondition in childs)
            {
                AddPerformerConditions(addedPerformer.Entity, performerPerformerCondition);
            }

            var res = await _unitOfWork.SaveAsync(CancellationToken.None);
            return addedPerformer.Entity;
        }

        private void AddPerformerConditions(Performer performer,PerformerCondition performerCondition)
        {
            performerCondition.Performer = performer;
            var addedPerformerCondition = _unitOfWork.Set<PerformerCondition>().Add(performerCondition);

            foreach (var child in performerCondition.Childs??new List<PerformerCondition>())
            {
                child.Parent = addedPerformerCondition.Entity;
                AddPerformerConditions(performer,child);
            }
        }
    }

    public class WorkflowShapesWithDetails
    {
        public Guid Guid { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int? CreatedBy { get; set; }
        public int? DocumentId { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Description { get; set; }
        public string UniqueName { get; set; }
        public string WorkflowName { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
        public virtual ICollection<Pool> Pools { get; set; } = default!;
        public virtual ICollection<Process> Processes { get; set; } = default!;
    }
}