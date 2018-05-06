using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EmployeeVacationCalendar.Repository.Migrations
{
#pragma warning disable 1591
	public partial class Initial : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Calendar",
				columns: table => new
				{
					Date = table.Column<DateTime>(nullable: false),
					Day = table.Column<int>(nullable: false),
					DayOfWeek = table.Column<int>(nullable: false),
					DayOfYear = table.Column<int>(nullable: false),
					Description = table.Column<string>(nullable: true),
					IsHoliday = table.Column<bool>(nullable: false),
					Month = table.Column<int>(nullable: false),
					Quarter = table.Column<int>(nullable: false),
					Week = table.Column<int>(nullable: false),
					WeekOfMonth = table.Column<int>(nullable: false),
					Year = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Calendar", x => x.Date);
				});

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Name = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "UserRoles",
				columns: table => new
				{
					UserId = table.Column<int>(nullable: false),
					RoleId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
				});

			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					Password = table.Column<string>(nullable: true),
					UserName = table.Column<string>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Employees",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					FirstName = table.Column<string>(nullable: true),
					LastName = table.Column<string>(nullable: true),
					UserId = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Employees", x => x.Id);
					table.ForeignKey(
						name: "FK_Employees_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Vacations",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false)
						.Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
					DateFrom = table.Column<DateTime>(nullable: false),
					DateTo = table.Column<DateTime>(nullable: false),
					EmployeeId = table.Column<int>(nullable: false),
					Status = table.Column<int>(nullable: false),
					Type = table.Column<int>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Vacations", x => x.Id);
					table.ForeignKey(
						name: "FK_Vacations_Employees_EmployeeId",
						column: x => x.EmployeeId,
						principalTable: "Employees",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Employees_UserId",
				table: "Employees",
				column: "UserId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Vacations_EmployeeId",
				table: "Vacations",
				column: "EmployeeId");

			migrationBuilder.Sql("INSERT INTO dbo.Roles(Name) VALUES('User')");
			migrationBuilder.Sql("INSERT INTO dbo.Roles(Name) VALUES('Admin')");

			migrationBuilder.Sql("INSERT INTO dbo.Users (UserName, Password) VALUES('User', 'pass')");
			migrationBuilder.Sql("INSERT INTO dbo.Users (UserName, Password) VALUES('Admin', 'pass')");
			migrationBuilder.Sql("INSERT INTO dbo.UserRoles (UserId, RoleId) VALUES(1, 1)");
			migrationBuilder.Sql("INSERT INTO dbo.UserRoles (UserId, RoleId) VALUES(2, 2)");

			migrationBuilder.Sql("INSERT INTO dbo.Employees(UserId, FirstName, LastName) VALUES(1, 'First', 'Last')");
			migrationBuilder.Sql("INSERT INTO dbo.Employees(UserId, FirstName, LastName) VALUES(2, 'Admin', 'Last')");
			migrationBuilder.Sql(@"CREATE FUNCTION Computus
						-- Computus (Latin for computation) is the calculation of the date of
						-- Easter in the Christian calendar
						-- http://en.wikipedia.org/wiki/Computus
						-- I'm using the Meeus/Jones/Butcher Gregorian algorithm
						(
							@Y INT -- The year we are calculating easter sunday for
						)
						RETURNS DATETIME
						AS
						BEGIN
						DECLARE
						@a INT,
						@b INT,
						@c INT,
						@d INT,
						@e INT,
						@f INT,
						@g INT,
						@h INT,
						@i INT,
						@k INT,
						@L INT,
						@m INT

						SET @a = @Y % 19
						SET @b = @Y / 100
						SET @c = @Y % 100
						SET @d = @b / 4
						SET @e = @b % 4
						SET @f = (@b + 8) / 25
						SET @g = (@b - @f + 1) / 3
						SET @h = (19 * @a + @b - @d - @g + 15) % 30
						SET @i = @c / 4
						SET @k = @c % 4
						SET @L = (32 + 2 * @e + 2 * @i - @h - @k) % 7
						SET @m = (@a + 11 * @h + 22 * @L) / 451
						RETURN(DATEADD(month, ((@h + @L - 7 * @m + 114) / 31)-1, cast(cast(@Y AS VARCHAR) AS Datetime)) + ((@h + @L - 7 * @m + 114) % 31))
						END
						GO


						ALTER TABLE Calendar
						-- In Celkoish style I'm manic about constraints (Never use em ;-))
						-- http://www.celko.com/

						ADD CONSTRAINT [Calendar_ck] CHECK (  ([Year] > 1900)
						AND ([Quarter] BETWEEN 1 AND 4)
						AND ([Month] BETWEEN 1 AND 12)
						AND ([Week]  BETWEEN 1 AND 53)
						AND ([Day] BETWEEN 1 AND 31)
						AND ([DayOfYear] BETWEEN 1 AND 366)
						AND ([DayOfWeek] BETWEEN 1 AND 7)
						)
						GO

						SET DATEFIRST 1;
						-- I want my table to contain datedata acording to ISO 8601
						-- http://en.wikipedia.org/wiki/ISO_8601
						-- thus first day of a week is monday
						WITH Dates(Date)
						-- A recursive CTE that produce all dates between 1999 and 2020-12-31
						AS
						(
						SELECT cast('2000' AS DateTime) Date -- SQL Server supports the ISO 8601 format so this is an unambigious shortcut for 1999-01-01
						UNION ALL                            -- http://msdn2.microsoft.com/en-us/library/ms190977.aspx
						SELECT (Date + 1) AS Date
						FROM Dates
						WHERE
						Date < cast('2100' AS DateTime) -1
						),

						DatesAndThursdayInWeek(Date, Thursday)
						-- The weeks can be found by counting the thursdays in a year so we find
						-- the thursday in the week for a particular date
						AS
						(
						SELECT
						Date,
						CASE DATEPART(Weekday,Date)
						WHEN 1 THEN Date + 3
						WHEN 2 THEN Date + 2
						WHEN 3 THEN Date + 1
						WHEN 4 THEN Date
						WHEN 5 THEN Date - 1
						WHEN 6 THEN Date - 2
						WHEN 7 THEN Date - 3
						END AS Thursday
						FROM Dates
						),

						Weeks(Week, Thursday)
						-- Now we produce the weeknumers for the thursdays
						AS
						(
						SELECT ROW_NUMBER() OVER(partition by year(Date) order by Date) Week, Thursday
						FROM DatesAndThursdayInWeek
						WHERE DATEPART(Weekday,Date) = 4
						)
						INSERT INTO Calendar
						(
							Date,
							Year,
							Quarter,
							Month,
							Week,
							Day,
							DayOfYear,
							DayOfWeek,
							WeekOfMonth,
							IsHoliday,
							Description
						)
						SELECT
						d.Date,
						YEAR(d.Date) AS Year,
						DATEPART(Quarter, d.Date) AS Quarter,
						MONTH(d.Date) AS Month,
						w.Week,
						DAY(d.Date) AS Day,
						DATEPART(DayOfYear, d.Date) AS DayOfYear,
						DATEPART(Weekday, d.Date) AS DayOfWeek,
						DATEPART(day, datediff(day, 0, d.Date)/7 * 7)/7 + 1,

						CASE
						-- Holidays in Norway
						-- For other countries and states: Wikipedia - List of holidays by country
						-- http://en.wikipedia.org/wiki/List_of_holidays_by_country
							WHEN (DATEPART(DayOfYear, d.Date) = 1)
							OR (DATEPART(DayOfYear, d.Date) = 6)
							OR (d.Date = dbo.Computus(YEAR(Date)))
							OR (d.Date = dbo.Computus(YEAR(Date)) + 1)
							OR (d.Date = dbo.Computus(YEAR(Date))+60)
							OR (MONTH(d.Date) = 5 AND DAY(d.Date) = 1)
							OR (MONTH(d.Date) = 6 AND DAY(d.Date) = 22)
							OR (MONTH(d.Date) = 6 AND DAY(d.Date) = 25)
							OR (MONTH(d.Date) = 8 AND DAY(d.Date) = 5)
							OR (MONTH(d.Date) = 8 AND DAY(d.Date) = 15)
							OR (MONTH(d.Date) = 10 AND DAY(d.Date) = 8)
							OR (MONTH(d.Date) = 11 AND DAY(d.Date) = 1)
							OR (MONTH(d.Date) = 12 AND DAY(d.Date) = 25)
							OR (MONTH(d.Date) = 12 AND DAY(d.Date) = 26)
							THEN 1
							ELSE 0
						END IsHoliday,
						CASE
						-- Description of holidays in Croatia
							WHEN (DATEPART(DayOfYear, d.Date) = 1)				THEN 'New Year''s Day'
							WHEN (DATEPART(DayOfYear, d.Date) = 6)				THEN 'Epiphany'
							WHEN (d.Date = dbo.Computus(YEAR(Date)))			THEN 'Easter Sunday'
							WHEN (d.Date = dbo.Computus(YEAR(Date)) + 1)		THEN 'Easter Monday'
							WHEN (d.Date = dbo.Computus(YEAR(Date))+60)			THEN 'Corpus Christi'
							WHEN (MONTH(d.Date) = 5 AND DAY(d.Date) = 1)		THEN 'Labour day'
							WHEN (MONTH(d.Date) = 6 AND DAY(d.Date) = 22)		THEN 'Anti-Fascist Struggle Day'
							WHEN (MONTH(d.Date) = 6 AND DAY(d.Date) = 25)		THEN 'Statehood Day'
							WHEN (MONTH(d.Date) = 8 AND DAY(d.Date) = 5)		THEN 'Victory and Homeland Thanksgiving Day and the Day of Croatian defenders'
							WHEN (MONTH(d.Date) = 8 AND DAY(d.Date) = 15)		THEN 'Assumption of Mary'
							WHEN (MONTH(d.Date) = 10 AND DAY(d.Date) = 8)		THEN 'Independence Day'
							WHEN (MONTH(d.Date) = 11 AND DAY(d.Date) = 1)		THEN 'All Saint''s Day'
							WHEN (MONTH(d.Date) = 12 AND DAY(d.Date) = 25)		THEN 'Cristmas day'
							WHEN (MONTH(d.Date) = 12 AND DAY(d.Date) = 26)		THEN 'St. Stephen''s Day'
						END Description

						FROM DatesAndThursdayInWeek d
						-- This join is for getting the week into the result set
								inner join Weeks w
								on d.Thursday = w.Thursday

						OPTION(MAXRECURSION 0)
						GO

						CREATE FUNCTION Numbers
						(
						@AFrom INT,
						@ATo INT,
						@AIncrement INT
						)
						RETURNS @RetNumbers TABLE
						(
						[Number] int PRIMARY KEY NOT NULL
						)
						AS
						BEGIN
						WITH Numbers(n)
						AS
						(
						SELECT @AFrom AS n
						UNION ALL
						SELECT (n + @AIncrement) AS n
						FROM Numbers
						WHERE
						n < @ATo
						)
						INSERT @RetNumbers
						SELECT n from Numbers
						OPTION(MAXRECURSION 0)
						RETURN;
						END
						GO

						CREATE FUNCTION iNumbers
						(
						@AFrom INT,
						@ATo INT,
						@AIncrement INT
						)
						RETURNS TABLE
						AS
						RETURN(
						WITH Numbers(n)
						AS
						(
						SELECT @AFrom AS n
						UNION ALL
						SELECT (n + @AIncrement) AS n
						FROM Numbers
						WHERE
						n < @ATo
						)
						SELECT n AS Number from Numbers
						)
						GO
						SELECT * FROM Calendar ORDER BY  Date
						DROP FUNCTION dbo.Computus
						DROP FUNCTION dbo.Numbers
						DROP FUNCTION dbo.iNumbers");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Calendar");

			migrationBuilder.DropTable(
				name: "Roles");

			migrationBuilder.DropTable(
				name: "UserRoles");

			migrationBuilder.DropTable(
				name: "Vacations");

			migrationBuilder.DropTable(
				name: "Employees");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
