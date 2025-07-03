using MediaFlow.Storage.EF;
using MediaFlow.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MediaFlow.Storage
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly AppDbContext _context;
		private readonly DbSet<T> _dbSet;

		public Repository(AppDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		public async Task<T?> GetByIdAsync(object id) =>
			await _dbSet.FindAsync(id);

		public async Task<IEnumerable<T>> GetAllAsync() =>
			await _dbSet.ToListAsync();

		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
			await _dbSet.Where(predicate).ToListAsync();

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public async Task UpdateAsync(T entity)
		{
			_dbSet.Update(entity);
			await Task.CompletedTask;
		}

		public async Task DeleteAsync(T entity)
		{
			_dbSet.Remove(entity);
			await Task.CompletedTask;
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}

}
