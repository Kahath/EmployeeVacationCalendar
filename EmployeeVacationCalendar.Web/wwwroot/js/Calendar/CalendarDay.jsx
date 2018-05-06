class CalendarDay extends React.Component {
	constructor(props) {
		super(props);

		this.setDialog = this.setDialog.bind(this);
	}

	setDialog(employeeId, vacationId, dateStart, dateEnd) {
		$(".dialogForm").hide();
		$(".vacationDialogForm").show();

		if (vacationId !== 0) {
			$("#vacationDialog .modal-footer input[name=Delete]").show();
			$.ajax({
				url: window.location.origin + "/Vacation/Get/" + vacationId,
				success: function (data) {
					$(".js-date-from").data("pickadate").set("select", moment(data.dateFrom).format("DD.MM.YYYY"));
					$(".js-date-to").data("pickadate").set("select", moment(data.dateTo).format("DD.MM.YYYY"));
					$(".btn-group .dropdown-toggle").val(data.type);

					var selector = ".dropdown-menu li a[value=" + data.type + "]";
					$(".btn-group .dropdown-toggle").text($(selector).text());
					$(".btn-group .dropdown-toggle").attr("value", data.type);
				}
			});
		}
		else {
			$("#vacationDialog .modal-footer input[name=Delete]").hide();
			$(".js-date-from").data("pickadate").set("select", dateStart);
			$(".js-date-to").data("pickadate").set("select", dateStart);
		}

		$("#vacationDialog .modal-footer input[name=Submit]").unbind("click");
		$("#vacationDialog .modal-footer input[name=Submit]").click({ employeeId: employeeId },
			function () {
				var vacationType = parseInt($(".dropdown-toggle").val());
				if (vacationType === undefined || vacationType === 0 || isNaN(vacationType))
					alert("Wrong vacation type");
				else {
					var dateFrom = $(".js-date-from").data("pickadate").get("select", "dd.mm.yyyy");
					var dateTo = $(".js-date-to").data("pickadate").get("select", "dd.mm.yyyy");

					var isInsert = vacationId === 0;
					var action = isInsert ? "/Add" : "/Update";
					var path = "/Vacation" + action;
					var method = isInsert ? "POST" : "PUT";

					if (moment(dateTo) < moment(dateFrom))
						alert("Date from cannot be greater than date to");
					else {
						$.ajax({
							url: window.location.origin + path,
							type: method,
							dataType: "json",
							data: {
								Id: vacationId,
								type: vacationType,
								dateFrom: dateFrom,
								dateTo: dateTo,
								employeeId: employeeId
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
					}
				}
			});

		$("#vacationDialog .modal-footer input[name=Delete]").unbind("click");
		$("#vacationDialog .modal-footer input[name=Delete]").click({ employeeId: employeeId, vacationId: vacationId },
			function deleteVacation() {
				$.ajax({
					url: window.location.origin + "/Vacation/Delete/" + vacationId,
					type: "DELETE",
					dataType: "json",
					success: function (data) {
						$('#vacationDialog').modal('hide');
						employeesCalendar.callback();
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
		var cssClass = "calendarDay noselect";
		const toggle = "modal";
		const target = "#vacationDialog";
		const employeeId = this.props.employeeId;
		const dateStart = moment(this.props.data.date).format("DD.MM.YYYY");
		const dateEnd = dateStart;
		var vacationId = 0;
		var canClick = false;

		if (this.props.data.isHoliday)
			cssClass += " holiday";
		else if (this.props.data.isWeekend)
			cssClass += " weekend";
		else if (this.props.data.vacationId !== 0) {
			if (this.props.data.vacationType === 2)
				cssClass += " sick"
			else
				cssClass += " vacation";
			canClick = true;
			vacationId = this.props.data.vacationId;
		}
		else {
			cssClass += " weekday";
			canClick = true;
		}

		const cssStil = {
			gridColumn: this.props.data.dayOfWeek
		}

		canClick = canClick && this.props.data.canEdit;
		if (canClick)
			cssClass += " pointer clickable";

		const ret = canClick ? (<button>{this.props.data.day}</button>) : (<span>{this.props.data.day}</span>);

		return (
			<div className={cssClass} style={cssStil} data-toggle={canClick ? toggle : undefined} data-target={canClick ? target : undefined} onClick={canClick ? () => this.setDialog(employeeId, vacationId, dateStart, dateEnd) : undefined}>
				<span>{this.props.data.day}</span>
			</div>
		);
	}
}