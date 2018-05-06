//var calendarDiv = $(".calendar");
//var year = parseInt(calendarDiv.attr("year"));
//var month = parseInt(calendarDiv.attr("month"));
//var path = calendarDiv.attr("path");
var year = moment().year();
var month = moment().month() + 1;
var page = 1;

ReactDOM.render(<EmployeesCalendar path="/Calendar/GetCalendar" year={year} month={month} page={page}/>, document.getElementsByClassName('calendar')[0])