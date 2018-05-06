class CalendarEmployee extends React.Component {
	constructor(props) {
		super(props);

		this.setDialog = this.setDialog.bind(this);
	}

	setDialog(employeeId) {
		$(".dialogForm").hide();
		$("#vacationDialog .modal-footer input[name=Delete]").hide();

		if (employeeId === 0 || employeeId === undefined || isNaN(employeeId))
			return alert("Employee must have id");

		$(".employeeDialogForm").show();
		$.ajax({
			url: window.location.origin + "/Employee/Get/" + employeeId,
			success: function (data) {
				$(".employeeFirstName").val(data.firstName);
				$(".employeeLastName").val(data.lastName);
			},
			error: function (data) {
				if (data.status === 500 || data.status === 401)
					alert(data.statusText);
				else
					alert(data.responseText);
			}
		});

		$("#vacationDialog .modal-footer input[name=Submit]").unbind("click");
		$("#vacationDialog .modal-footer input[name=Submit]").click({ employeeId: employeeId },
			function () {
				var firstName = $(".employeeFirstName").val();
				var lastName = $(".employeeLastName").val();

				var path = "/Employee/Update/";
				var method = "PUT";

				$.ajax({
					url: window.location.origin + path,
					type: method,
					dataType: "json",
					data: {
						Id: employeeId,
						firstName: firstName,
						lastName: lastName,
					},
					success: function (data) {
						employeesCalendar.callback();
						$('#vacationDialog').modal('hide');
					},
					error: function (data) {
						if (data.status === 500 || data.status === 401)
							alert(data.statusText);
						else
							alert(data.responseText);
					}
				});
			});
	}

	render() {
		const canClick = this.props.data.canEdit;
		const employeeId = this.props.data.id;

		const toggle = "modal";
		const target = "#vacationDialog";
		const css = canClick && employeeId != 0 ? "pointer clickableUser" : "";

		return (
			<div>
				<div className="calendarY">
					<span className={css} data-toggle={canClick ? toggle : undefined} data-target={canClick ? target : undefined} onClick={canClick ? () => this.setDialog(employeeId) : undefined}>
						{this.props.data.name}
					</span>
					<CalendarMonth data={this.props.data.weeks} employeeId={employeeId} />
					<div className="yGap"> </div>
				</div>
			</div>
		);
	}
}