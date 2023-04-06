using System;
using static Arfware.ArfBlocks.Core.Models.EndpointModel;

namespace Arfware.ArfBlocks.Test.Models
{

	public class ApplicationTestDataModel
	{
		public Type TestConfigurationsType { get; set; }
	}

	public class TestModel
	{
		public string ObjectName { get; set; }
		public string ActionName { get; set; }
		public Type TestType { get; set; }
		public EndpointTypes EndpointType { get; set; }
	}
}