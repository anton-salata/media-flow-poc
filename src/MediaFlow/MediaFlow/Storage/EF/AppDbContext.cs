using MediaFlow.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaFlow.Storage.EF
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options) { }

		public DbSet<DidYouKnowItem> DidYouKnowItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DidYouKnowItem>().ToTable("DidYouKnow");

			base.OnModelCreating(modelBuilder);
		}
	}
}
