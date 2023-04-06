using System;
using System.Collections.Generic;
using TodoApp.Domain.Entities.Base;

namespace TodoApp.Domain.Entities
{
	public class Department : BaseEntity
	{
		public string Name { get; set; }
	}
}
