using System;

namespace DAL.Entities
{
	public interface IHasIntId
	{
		int Id { get; set; }
	}

	public interface IHasGuidId
	{
		Guid Id { get; set; }
	}

	/// <summary>
	/// Base class for entities.
	/// Contains <see cref="Id"/> property.
	/// </summary>
	public class BaseEntityGuid : BaseEntity, IHasGuidId
	{
		/// <summary>
		/// Gets and sets id.
		/// </summary>
		public virtual Guid Id { get; set; }
	}

	public class BaseEntityInt : BaseEntity, IHasIntId
	{
		public virtual int Id { get; set; }
	}

	public abstract class BaseEntity
	{
		/// <summary>
		/// 使用软删除
		/// </summary>
		public bool IsRemoved { get; set; }

		/// <summary>
		/// 删除时间
		/// </summary>
		public DateTime IsRemovedDate { get; set; }

		public void Remove()
		{
			this.IsRemoved = true;
			this.IsRemovedDate = DateTime.Now;
		}
	}
}