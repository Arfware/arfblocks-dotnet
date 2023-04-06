using System;
using System.Threading.Tasks;

namespace BusinessModules.Management.Infrastructure.Services
{
	public class ActivityLogService
	{
		// private readonly ActivityLogConnector _activityLogConnector;
		// public ManagementActivityLogService(ActivityLogConnector activityLogService)
		// {
		// 	_activityLogConnector = activityLogService;
		// }

		private async Task AddUserLog(Guid currentUserId, string userDisplayName, Guid objectId, string content)
		{
			// var friendlyActionName = ActivityLogConnector.GetActionFriendlyName(actionType);
			// var content = $"{objectName} {statement} {friendlyActionName}.";
			// await _activityLogConnector.AddUserLog(currentUserId, userDisplayName, objectId, UserModulesEnumContract.Management, DateTime.Now, content);
			await Task.FromResult(true);
		}

		// USER
		public async Task UserLogined(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Logined";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}

		// TASK
		public async Task TaskCreated(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Created Task";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}

		public async Task TaskUpdated(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Updated Task";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}

		public async Task TaskDeleted(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Deleted Task";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}

		public async Task TaskCompleted(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Completed Task";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}

		public async Task TaskRejected(Guid currentUserId, string userDisplayName, Guid objectId)
		{
			var content = "Rejected Task";
			await AddUserLog(currentUserId, userDisplayName, objectId, content);
		}
	}
}
