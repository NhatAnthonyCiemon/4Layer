using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Layer.Infrastructure.Helper
{
	public class ApplyAuditFields
	{
		private ApplyAuditFields() { }
		static public void ApplyAuditFieldsToEntities(IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> entries)
		{
			foreach (var entry in entries)
			{
				if (entry.Entity is _4Layer.Domain.Common.BaseEntity baseEntity)
				{
					switch (entry.State)
					{
						case Microsoft.EntityFrameworkCore.EntityState.Added:
							baseEntity.CreatedAt = DateTime.UtcNow;
							break;

						case Microsoft.EntityFrameworkCore.EntityState.Modified:
							baseEntity.UpdatedAt = DateTime.UtcNow;
							break;

						case Microsoft.EntityFrameworkCore.EntityState.Deleted:
							entry.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
							baseEntity.DeletedAt = DateTime.UtcNow;
							break;
					}
				}
			}
		}
	}
}
