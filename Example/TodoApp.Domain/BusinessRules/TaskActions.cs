using System;
using TodoApp.Domain.Entities;

namespace TodoApp.Domain.BusinessRules;

public static class TaskErrors
{
	public static string FOR_COMPLETE_CURRENT_USER_DEPARTMENT_MUST_EQUAL_TO_TASK_ASSIGNED_DEPARTMENT = "FOR_COMPLETE_CURRENT_USER_DEPARTMENT_MUST_EQUAL_TO_TASK_ASSIGNED_DEPARTMENT";
	public static string FOR_COMPLETE_TASK_STATUS_MUST_BE_PENDING = "FOR_COMPLETE_TASK_STATUS_MUST_BE_PENDING";
}

public class TaskActions
{
	public static class Complete
	{
		public static (bool HasError, string ErrorCode) CheckActor(TodoTask task, User user)
		{
			if (!TaskBusinessRules.IsUserDepartmentEqualsTaskAssignedDepartment(task, user))
				return (true, TaskErrors.FOR_COMPLETE_CURRENT_USER_DEPARTMENT_MUST_EQUAL_TO_TASK_ASSIGNED_DEPARTMENT);

			return (false, null);
		}

		public static (bool HasError, string ErrorCode) CheckDomain(TodoTask task, User user)
		{
			if (!TaskBusinessRules.IsTaskStatusPending(task))
				return (true, TaskErrors.FOR_COMPLETE_TASK_STATUS_MUST_BE_PENDING);

			return (false, null);
		}
	}
}
