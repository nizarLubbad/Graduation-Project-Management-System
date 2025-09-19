using AutoMapper;
using GPMS.DTOS.Project;
using GPMS.DTOS.Task;
using GPMS.Interfaces;
using GPMS.Models;
using GPMS.Models.Enums;
using Microsoft.EntityFrameworkCore;
using CreateTaskDto = GPMS.DTOS.Task.CreateTaskDto;

namespace GPMS.Services
{
    //public class KanbanTaskService : IKanbanTaskService
    //{
    //    private readonly ITaskRepository _taskRepository;
    //    //private readonly AppDbContext _context;
    //    private readonly IMapper _mapper;

    //    public KanbanTaskService(ITaskRepository taskRepository,IMapper mapper )
    //    {
    //        _taskRepository = taskRepository;
    //        _mapper = mapper;
    //        //_context = context;

    //    }

    //    public async Task<KanbanTask> CreateAsync(CreateTaskDto dto)
    //    {
    //        var students = await _context.Students
    //            .Where(s => dto.AssignedStudentIds.Contains(s.StudentId))
    //            .ToListAsync();

    //        var task = new KanbanTask
    //        {
    //            Title = dto.Title,
    //            Description = dto.Description,
    //            DueDate = dto.DueDate,
    //            TeamId = dto.TeamId,
    //            Priority = (TaskPriorityEnum)dto.Priority,
    //            Status = TaskStatusEnum.ToDo,
    //            AssignedStudents = students
    //        };

    //        return await _taskRepository.AddAsync(task);
    //    }

    //    public async Task<KanbanTask?> UpdateAsync(long id, UpdateTaskDto dto)
    //    {
    //        var task = await _taskRepository.GetByIdAsync(id);
    //        if (task == null) return null;

    //        if (!string.IsNullOrEmpty(dto.Title)) task.Title = dto.Title;
    //        if (!string.IsNullOrEmpty(dto.Description)) task.Description = dto.Description;
    //        if (dto.DueDate.HasValue) task.DueDate = dto.DueDate.Value;
    //        if (dto.Priority.HasValue) task.Priority = dto.Priority.Value;
    //        if (dto.Status.HasValue) task.Status = dto.Status.Value;

    //        if (dto.AssignedStudentIds != null)
    //        {
    //            var students = await _context.Students
    //                .Where(s => dto.AssignedStudentIds.Contains(s.StudentId))
    //                .ToListAsync();
    //            task.AssignedStudents = students;
    //        }

    //        await _taskRepository.UpdateAsync(task);
    //        return task;
    //    }

    //    public async Task<KanbanTask?> GetByIdAsync(long id)
    //    {
    //        return await _taskRepository.GetByIdAsync(id);
    //    }

    //    public async Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId)
    //    {
    //        return await _taskRepository.GetByTeamIdAsync(teamId);
    //    }

    //    public async Task<bool> DeleteAsync(long id)
    //    {
    //        var task = await _taskRepository.GetByIdAsync(id);
    //        if (task == null) return false;

    //        await _taskRepository.DeleteAsync(task);
    //        return true;
    //    }

    //    public async Task<KanbanTask?> ChangeStatusAsync(long id, TaskStatusEnum newStatus)
    //    {
    //        var task = await _taskRepository.GetByIdAsync(id);
    //        if (task == null) return null;

    //        task.Status = newStatus;
    //        await _taskRepository.UpdateAsync(task);
    //        return task;
    //    }

    //    public async Task<KanbanTask?> UpdateAssignedStudentsAsync(long id, List<long> assignedStudentIds)
    //    {
    //        var task = await _taskRepository.GetByIdAsync(id);
    //        if (task == null) return null;

    //        var students = await _context.Students
    //            .Where(s => assignedStudentIds.Contains(s.StudentId))
    //            .ToListAsync();

    //        task.AssignedStudents = students;
    //        await _taskRepository.UpdateAsync(task);
    //        return task;
    //    }

    //    public async Task<KanbanTask?> ChangePriorityAsync(long id, TaskPriorityEnum newPriority)
    //    {
    //        var task = await _taskRepository.GetByIdAsync(id);
    //        if (task == null) return null;

    //        task.Priority = newPriority;
    //        await _taskRepository.UpdateAsync(task);
    //        return task;
    //    }
    //}

    public class KanbanTaskService : IKanbanTaskService
    {
        private readonly IBaseRepository<KanbanTask> _taskRepository;
        private readonly IBaseRepository<Student> _studentRepository;
        private readonly IMapper _mapper;

        public KanbanTaskService(
            IBaseRepository<KanbanTask> taskRepository,
            IBaseRepository<Student> studentRepository,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new KanbanTask
            {
                Title = dto.Title,
                Description = dto.Description,
                TeamId = dto.TeamId,
                Status = Models.Enums.TaskStatus.ToDo,
                Priority = TaskPriority.Medium,
                DueDate = null,
                CreatedAt = DateTime.UtcNow
            };

            var saved = await _taskRepository.AddAsync(task);
            return _mapper.Map<TaskDto>(saved);
        }

     
        public async Task<TaskDto?> GetByIdAsync(long taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            return task == null ? null : _mapper.Map<TaskDto>(task);
        }

        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }

        public async Task<TaskDto?> UpdateAsync(long taskId, UpdateTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return null;

            if (!string.IsNullOrEmpty(dto.Title))
                task.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                task.Description = dto.Description;

            if (dto.DueDate.HasValue)
                task.DueDate = dto.DueDate.Value;

            if (dto.Priority.HasValue)
                task.Priority = dto.Priority.Value;

            if (dto.TeamId.HasValue)
                task.TeamId = dto.TeamId.Value;

            if (dto.AssignedStudentNames != null && dto.AssignedStudentNames.Any())
            {
                var students = await _studentRepository.GetAllAsync();
                var matched = students
                    .Where(s => dto.AssignedStudentNames.Contains(s.Name))
                    .ToList();

                task.AssignedStudents = matched;
            }

            var updated = await _taskRepository.UpdateAsync(task);
            return _mapper.Map<TaskDto>(updated);
        }

        public async Task<TaskDto?> UpdateStatusAsync(UpdateTaskStatusDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(dto.TaskId);
            if (task == null) return null;

            if (!string.IsNullOrEmpty(dto.Status) &&
                Enum.TryParse<Models.Enums.TaskStatus>(dto.Status, true, out var newStatus))
            {
                task.Status = newStatus;
            }

            var updated = await _taskRepository.UpdateAsync(task);
            return _mapper.Map<TaskDto>(updated);
        }

        public async Task<bool> DeleteAsync(long taskId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(task);
            return true;
        }

        
    }


}
