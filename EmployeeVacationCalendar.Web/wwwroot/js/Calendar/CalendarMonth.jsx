class CalendarMonth extends React.Component {
	render() {
		const employeeId = this.props.employeeId;
		var weekNodes = this.props.data.map(function (week) {
			return (
				<CalendarWeek key={week.id} data={week} employeeId={employeeId} />
			);
		});

		return (
			<div className="month">
				{weekNodes}
			</div>
		);
	}
}
