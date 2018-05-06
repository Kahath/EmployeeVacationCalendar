CREATE FUNCTION Computus
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
DROP FUNCTION dbo.iNumbers