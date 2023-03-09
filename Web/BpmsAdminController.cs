using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Bpms.WorkflowEngine.Databases.Entities;
using Bpms.WorkflowEngine.Services;

namespace Bpms.WorkflowEngine.Controllers
{
    [Route("api/bpms/[controller]/[action]")]
    public class BpmsAdminController : ControllerBase
    {
        private readonly IBpmsAdminService _bpmsAdminService;
        private readonly IValidator<BpmsAdminService.CreateWorkflowModel> _createWorkflowValidator;
        
        public BpmsAdminController(IBpmsAdminService bpmsAdminService, IValidator<BpmsAdminService.CreateWorkflowModel> createWorkflowValidator)
        {
            _bpmsAdminService = bpmsAdminService;
            _createWorkflowValidator = createWorkflowValidator;
        }


        [HttpGet]
        public async Task<IActionResult> GetWorkflowWithDetails(int id)
        {
             return Ok(await _bpmsAdminService.GetWorkflowWithDetails(id));
        }

        [HttpGet]
        public async Task<IActionResult> GeAllWorkflows()
        {
            return Ok(await _bpmsAdminService.GeAllWorkflows());
        }


        [HttpPost]
        public async Task<IActionResult> CreateWorkflow([FromForm] BpmsAdminService.CreateWorkflowModel model)
        {
            var validate = await _createWorkflowValidator.ValidateAsync(model);
            if (validate.IsValid)
            {
                var res = await _bpmsAdminService.CreateWorkflow(model);
                return Ok(res);
            }
            else
            {
                validate.AddToModelState(this.ModelState);
                return ValidationProblem(ModelState);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddPerformer([FromBody] Performer model)
        {
           var a =await  _bpmsAdminService.AddPerformer(model);
           return Ok(a);
        }
    }
}

public static class fluentValidationExtentions
{
    public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
    {
        foreach (var error in result.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }

}