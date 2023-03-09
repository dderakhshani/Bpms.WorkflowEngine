using AutoMapper;

namespace Bpms.WorkflowEngine.Infrastructure.Mappings
{
    public interface IMapFrom<T>
    {   
        void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
