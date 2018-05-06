var employeesCalendar = {};

class EmployeesCalendar extends React.Component {
	constructor(props) {
		super(props);
		this.state = {
			path: props.path,
			year: props.year,
			month: props.month,
			page: props.page,
			maxPages: 1,
			error: null,
			isLoaded: false,
			items: []
		};

		this.previousMonth = this.previousMonth.bind(this);
		this.posteriorMonth = this.posteriorMonth.bind(this);
		this.previousYear = this.previousYear.bind(this);
		this.posteriorYear = this.posteriorYear.bind(this);
		this.previousPage = this.previousPage.bind(this);
		this.posteriorPage = this.posteriorPage.bind(this);

		this.fetchData = this.fetchData.bind(this);
	}

	componentDidMount() {
		this.fetchData(year, month, page);
	}

	componentWillMount() {
		employeesCalendar.callback = () => {
			this.fetchData(this.state.year, this.state.month, this.state.page);
		}
	}

	fetchData(year, month, page) {
		fetch(document.location.origin + this.state.path + "/" + year + "/" + month + "/" + page,
			{
				credentials: "same-origin",
			})
			.then((res) => res.json())
			.then((result) => {
				this.setState({
					isLoaded: true,
					maxPages: result.maxPages,
					items: result
				});
			},
			// Note: it's important to handle errors here
			// instead of a catch() block so that we don't swallow
			// exceptions from actual bugs in components.
			(error) => {
				this.setState({
					isLoaded: true,
					error
				});
			});
	}

	previousYear() {
		var year = this.state.year;
		var month = this.state.month;
		var page = this.state.page;
		if (year <= 2000)
			year = 2000;
		else
			year -= 1;

		this.state.year = year;
		this.fetchData(year, month, page);
	}

	posteriorYear() {
		var year = this.state.year;
		var month = this.state.month;
		var page = this.state.page;
		if (year >= 2099)
			year = 2099;
		else
			year += 1;

		this.state.year = year;
		this.fetchData(year, month, page);
	}

	previousMonth() {
		var month = this.state.month;
		var year = this.state.year;
		var page = this.state.page;
		if (month <= 1) {
			if (year <= 2000)
				year = 2000;
			else {
				year -= 1;
				month = 12;
			}
		}
		else
			month -= 1;

		this.state.month = month;
		this.state.year = year;
		this.fetchData(year, month, page);
	}

	posteriorMonth() {
		var month = this.state.month;
		var year = this.state.year;
		var page = this.state.page;
		if (month >= 12) {
			if (year >= 2099)
				year = 2099;
			else {
				year += 1;
				month = 1;
			}
		}
		else
			month += 1;

		this.state.month = month;
		this.state.year = year;
		this.fetchData(year, month, page);
	}

	previousPage() {
		var year = this.state.year;
		var month = this.state.month;
		var page = this.state.page;
		if (page <= 1)
			page = 1;
		else
			page -= 1;

		this.state.page = page;
		this.fetchData(year, month, page);
	}

	posteriorPage() {
		var year = this.state.year;
		var month = this.state.month;
		var page = this.state.page;

		if (page >= this.state.maxPages)
			page = this.state.maxPages;
		else
			page += 1;

		this.state.page = page;
		this.fetchData(year, month, page);
	}

	render() {
		const { error, isLoaded, items } = this.state;
		if (error) {
			return <div>{error.message}</div>;
		} else if (!isLoaded) {
			return <div>Loading...</div>;
		}
		else {
			const currentYear = this.state.year;
			const toggle = "modal";
			const target = "#vacationDialog";

			var yAxisNodes = items.calendarY.map(function (yData) {
				return (
					<CalendarEmployee key={yData.id} data={yData} />
				)
			});

			return (
				<div>
					<div className="calendarHeader">
						<span>Year</span>
						<span className="navigationButton noselect fas fa-arrow-circle-left" onClick={this.previousYear}></span>
						<span>{this.state.year}</span>
						<span className="navigationButton noselect fas fa-arrow-circle-right" onClick={this.posteriorYear}></span>
						<span>Month</span>
						<span className="navigationButton noselect fas fa-arrow-circle-left" onClick={this.previousMonth}></span>
						<span>{this.state.month}</span>
						<span className="navigationButton noselect fas fa-arrow-circle-right" onClick={this.posteriorMonth}></span>
						<span>Page</span>
						<span className="navigationButton noselect fas fa-arrow-circle-left" onClick={this.previousPage}></span>
						<span>{this.state.page}</span>
						<span className="navigationButton noselect fas fa-arrow-circle-right" onClick={this.posteriorPage}></span>

					</div>
					<div className="calendar">
						{yAxisNodes}
					</div>
				</div>
			);
		}
	}
}
