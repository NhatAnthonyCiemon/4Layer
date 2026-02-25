using System;

namespace _4Layer.Domain.Common
{
	public abstract class BaseEntity
	{
		public Guid Id { get; private set; }

		public DateTime CreatedAt { get; set; }

		public DateTime? UpdatedAt { get; set; }

		public DateTime? DeletedAt { get; set; }

		protected BaseEntity()
		{
			Id = Guid.NewGuid();
		}

		public bool IsDeleted => DeletedAt.HasValue;
	}
}