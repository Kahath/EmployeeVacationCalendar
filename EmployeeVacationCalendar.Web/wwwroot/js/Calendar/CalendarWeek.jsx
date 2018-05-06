class CalendarWeek extends React.Component {
	render() {
		const employeeId = this.props.employeeId;
		var dayNodes = this.props.data.days.map(function (day) {
			return (
				<CalendarDay key={day.day} data={day} employeeId={employeeId} />
			);
		});
		return (
			<div className="week">
				{dayNodes}
			</div>
		);
	}
}