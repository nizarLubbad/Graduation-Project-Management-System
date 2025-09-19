using AutoMapper;
using GPMS.DTOS.Task;
using GPMS.Models;

namespace GPMS.MappingProfiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<KanbanTask,CreateTaskDto>();
            CreateMap<KanbanTask, UpdateTaskDto>();
        }
    }
}
