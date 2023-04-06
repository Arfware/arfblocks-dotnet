using System;

namespace TodoApp.Infrastructure.Services
{
	public class DocumentDbConfiguration
	{
		public string ConnectionString { get; set; }
		public string Database { get; set; }
	}
}