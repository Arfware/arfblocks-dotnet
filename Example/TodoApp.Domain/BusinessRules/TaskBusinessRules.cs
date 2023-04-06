using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.BusinessRules;

public class TaskBusinessRules
{

	// Logics
	public static bool IsTaskStatusPending(TodoTask task)
	{
		return task.Status == TodoTaskStatus.Pending;
	}

	public static bool IsUserDepartmentEqualsTaskAssignedDepartment(TodoTask task, User user)
	{
		return task.AssignedDepartmentId == user.DepartmentId;
	}

	public static bool IsUserTaskOwner(TodoTask task, Guid userId)
	{
		return task.CreatedById == userId;
	}

	/// RULES ///

	public static (bool HasError, string ErrorCode) CheckForComplete(TodoTask task, User currentUser)
	{
		if (!TaskBusinessRules.IsTaskStatusPending(task))
			return (true, "FOR_COMPLETE_TASK_STATUS_MUST_BE_PENDING");

		if (!TaskBusinessRules.IsUserDepartmentEqualsTaskAssignedDepartment(task, currentUser))
			return (true, "FOR_COMPLETE_CURRENT_USER_DEPARTMENT_MUST_EQUAL_TO_TASK_ASSIGNED_DEPARTMENT");

		return (false, null);
	}

	public static (bool HasError, string ErrorCode) CheckForReject(TodoTask task, User currentUser)
	{
		if (!TaskBusinessRules.IsTaskStatusPending(task))
			return (true, "FOR_COMPLETE_TASK_STATUS_MUST_BE_PENDING");

		if (!TaskBusinessRules.IsUserDepartmentEqualsTaskAssignedDepartment(task, currentUser))
			return (true, "FOR_COMPLETE_CURRENT_USER_DEPARTMENT_MUST_EQUAL_TO_TASK_ASSIGNED_DEPARTMENT");

		return (false, null);
	}

	public static (bool HasError, string ErrorCode) CheckForUpdate(TodoTask task, Guid currentUserId)
	{
		if (!TaskBusinessRules.IsUserTaskOwner(task, currentUserId))
			return (true, "FOR_UPDATE_CURRENT_USER_MUST_BE_TASK_CREATOR");

		if (!TaskBusinessRules.IsTaskStatusPending(task))
			return (true, "FOR_UPDATE_TASK_STATUS_MUST_BE_PENDING");

		return (false, null);
	}

	public static (bool HasError, string ErrorCode) CheckForDelete(TodoTask task, Guid currentUserId)
	{
		if (!TaskBusinessRules.IsUserTaskOwner(task, currentUserId))
			return (true, "FOR_DELETE_CURRENT_USER_MUST_BE_TASK_CREATOR");

		return (false, null);
	}
}