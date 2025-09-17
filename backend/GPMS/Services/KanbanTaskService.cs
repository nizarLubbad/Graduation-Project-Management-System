using GPMS.DTOS.Task;
using GPMS.Interfaces;
using GPMS.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatusEnum = GPMS.Models.Enums.TaskStatus;
using TaskPriorityEnum = GPMS.Models.Enums.TaskPriority;

namespace GPMS.Services
{
    public class KanbanTaskService : IKanbanTaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly AppDbContext _context;

        public KanbanTaskService(ITaskRepository taskRepository, AppDbContext context)
        {
            _taskRepository = taskRepository;
            _context = context;
        }

        public async Task<KanbanTask> CreateAsync(CreateTaskDto dto)
        {
            var students = await _context.Students
                .Where(s => dto.AssignedStudentIds.Contains(s.StudentId))
                .ToListAsync();

            var task = new KanbanTask
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                TeamId = dto.TeamId,
                Priority = (TaskPriorityEnum)dto.Priority,
                Status = TaskStatusEnum.ToDo,
                AssignedStudents = students
            };

            return await _taskRepository.AddAsync(task);
        }

        public async Task<KanbanTask?> UpdateAsync(long id, UpdateTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) task.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) task.Description = dto.Description;
            if (dto.DueDate.HasValue) task.DueDate = dto.DueDate.Value;
            if (dto.Priority.HasValue) task.Priority = dto.Priority.Value;
            if (dto.Status.HasValue) task.Status = dto.Status.Value;

            if (dto.AssignedStudentIds != null)
            {
                var students = await _context.Students
                    .Where(s => dto.AssignedStudentIds.Contains(s.StudentId))
                    .ToListAsync();
                task.AssignedStudents = students;
            }

            await _taskRepository.UpdateAsync(task);
            return task;
        }

        public async Task<KanbanTask?> GetByIdAsync(long id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<KanbanTask>> GetByTeamIdAsync(long teamId)
        {
            return await _taskRepository.GetByTeamIdAsync(teamId);
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            await _taskRepository.DeleteAsync(task);
            return true;
        }

        public async Task<KanbanTask?> ChangeStatusAsync(long id, TaskStatusEnum newStatus)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.Status = newStatus;
            await _taskRepository.UpdateAsync(task);
            return task;
        }

        public async Task<KanbanTask?> UpdateAssignedStudentsAsync(long id, List<long> assignedStudentIds)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            var students = await _context.Students
                .Where(s => assignedStudentIds.Contains(s.StudentId))
                .ToListAsync();

            task.AssignedStudents = students;
            await _taskRepository.UpdateAsync(task);
            return task;
        }

        public async Task<KanbanTask?> ChangePriorityAsync(long id, TaskPriorityEnum newPriority)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return null;

            task.Priority = newPriority;
            await _taskRepository.UpdateAsync(task);
            return task;
        }
    }
}
