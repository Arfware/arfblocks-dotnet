using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoApp.Infrastructure.Migrations
{
	public partial class CreatedDefaultObjects : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Departments",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Departments", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
					DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
					table.ForeignKey(
						name: "FK_Users_Departments_DepartmentId",
						column: x => x.DepartmentId,
						principalTable: "Departments",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
				});

			migrationBuilder.CreateTable(
				name: "Tasks",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Status = table.Column<int>(type: "int", nullable: false),
					StatusChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					AssignedDepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
					IsDeleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
					UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
					DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Tasks", x => x.Id);
					table.ForeignKey(
						name: "FK_Tasks_Departments_AssignedDepartmentId",
						column: x => x.AssignedDepartmentId,
						principalTable: "Departments",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
					table.ForeignKey(
						name: "FK_Tasks_Users_CreatedById",
						column: x => x.CreatedById,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.NoAction);
				});

			migrationBuilder.InsertData(
				table: "Departments",
				columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "Name", "UpdatedAt" },
				values: new object[,]
				{
					{ new Guid("927dbce3-f162-4e80-8991-4c71d7aa7153"), new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7213), null, false, "Human Resources", null },
					{ new Guid("423e95a1-44ce-4b4c-bffe-37d4548e51bd"), new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7468), null, false, "Sales", null },
					{ new Guid("f20c58c7-52d6-4975-aef1-fd5f9fafc841"), new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7471), null, false, "Marketing", null },
					{ new Guid("138ff80c-4139-4428-a1e0-2a475aa969c4"), new DateTime(2021, 12, 30, 9, 1, 14, 890, DateTimeKind.Utc).AddTicks(7473), null, false, "Information Technologies", null }
				});

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "CreatedAt", "DeletedAt", "DepartmentId", "Email", "FirstName", "IsDeleted", "LastName", "UpdatedAt" },
				values: new object[] { new Guid("f973d74b-b7df-40a6-a530-017dcdd870e7"), new DateTime(2021, 12, 30, 9, 1, 14, 891, DateTimeKind.Utc).AddTicks(5849), null, new Guid("927dbce3-f162-4e80-8991-4c71d7aa7153"), "mary@company.com", "Mary", false, "Gleen", null });

			migrationBuilder.InsertData(
				table: "Users",
				columns: new[] { "Id", "CreatedAt", "DeletedAt", "DepartmentId", "Email", "FirstName", "IsDeleted", "LastName", "UpdatedAt" },
				values: new object[] { new Guid("3f05215c-b48e-479f-985d-001f2bdf2b7b"), new DateTime(2021, 12, 30, 9, 1, 14, 891, DateTimeKind.Utc).AddTicks(5860), null, new Guid("423e95a1-44ce-4b4c-bffe-37d4548e51bd"), "john@company.com", "John", false, "Doe", null });

			migrationBuilder.CreateIndex(
				name: "IX_Tasks_AssignedDepartmentId",
				table: "Tasks",
				column: "AssignedDepartmentId");

			migrationBuilder.CreateIndex(
				name: "IX_Tasks_CreatedById",
				table: "Tasks",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Users_DepartmentId",
				table: "Users",
				column: "DepartmentId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Tasks");

			migrationBuilder.DropTable(
				name: "Users");

			migrationBuilder.DropTable(
				name: "Departments");
		}
	}
}
