using DAL.Interfaces;
using Domain.Entities;

namespace DAL.Repositories
{
    public class StatusRepository : IBaseRepository<StatusEntity>
    {
        private readonly DataContext _dbContext;

        public StatusRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(StatusEntity status)
        {
            await _dbContext.Statuses.AddAsync(status);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(StatusEntity status)
        {
            _dbContext.Statuses.Remove(status);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<StatusEntity> GetAll()
        {
            return _dbContext.Statuses;
        }

        public async Task<StatusEntity> Update(StatusEntity status)
        {
            _dbContext.Statuses.Update(status);
            await _dbContext.SaveChangesAsync();

            return status;
        }
    }
}
