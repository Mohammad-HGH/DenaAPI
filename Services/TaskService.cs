using DenaAPI.Interfaces;
using DenaAPI.Models;
using DenaAPI.Responses;
using Microsoft.EntityFrameworkCore;


namespace TasksApi.Services
{

    public class TaskService : ITaskService
    {
        private readonly DenadbContext denadbContext;

        public TaskService(DenadbContext denadbContext)
        {
            this.denadbContext = denadbContext;
        }

        public async Task<DeleteTaskResponse> DeleteTask(int taskId, int userId)
        {
            var task = await denadbContext.Tasks.FindAsync(taskId);

            if (task == null)
            {
                return new DeleteTaskResponse
                {
                    Success = false,
                    Error = "Task not found",
                    ErrorCode = "T01"
                };
            }

            if (task.UserId != userId)
            {
                return new DeleteTaskResponse
                {
                    Success = false,
                    Error = "You don't have access to delete this task",
                    ErrorCode = "T02"
                };
            }

            denadbContext.Tasks.Remove(task);

            var saveResponse = await denadbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new DeleteTaskResponse
                {
                    Success = true,
                    TaskId = task.Id
                };
            }

            return new DeleteTaskResponse
            {
                Success = false,
                Error = "Unable to delete task",
                ErrorCode = "T03"
            };
        }

        public async Task<GetTasksResponse> GetTasks(int userId)
        {
            var tasks = await denadbContext.Tasks.Where(o => o.UserId == userId).ToListAsync();

            return new GetTasksResponse { Success = true, Tasks = tasks };

        }

        public async Task<SaveTaskResponse> SaveTask(Task task)
        {
            if (task.Id == 0)
            {
                await denadbContext.Tasks.AddAsync(task);
            }
            else
            {
                var taskRecord = await denadbContext.Tasks.FindAsync(task.Id);

                taskRecord.IsCompleted = task.IsCompleted;
                taskRecord.Ts = task.Ts;
            }

            var saveResponse = await denadbContext.SaveChangesAsync();

            if (saveResponse >= 0)
            {
                return new SaveTaskResponse
                {
                    Success = true,
                    Task = task
                };
            }
            return new SaveTaskResponse
            {
                Success = false,
                Error = "Unable to save task",
                ErrorCode = "T05"
            };
        }
    }
}