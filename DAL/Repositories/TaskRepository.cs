using DAL.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DAL.Repositories
{
    public class TaskRepository : IBaseRepository<TaskEntity>
    {
        private readonly DataContext _dbContext;

        public TaskRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(TaskEntity entity)
        {
            await _dbContext.Tasks.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(TaskEntity entity)
        {
            _dbContext.Tasks.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<TaskEntity> GetAll()
        {
                return _dbContext.Tasks
                .Include(t => t.Status);
        }

        public async Task<TaskEntity> Update(TaskEntity entity)
        {
            _dbContext.Tasks.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
