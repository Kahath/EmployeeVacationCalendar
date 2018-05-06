$(".js-date-from").pickadate({ format: "dd.mm.yyyy", container: "body" });
$(".js-date-to").pickadate({ format: "dd.mm.yyyy", container: "body" });

$(function () {
	$(".dropdown-menu li a").click(function () {
		$(".btn-group .btn:first-child").text($(this).text());
		$(".btn-group .btn:first-child").val($(this).attr("value"));
	});
});

$(".dropdown-menu a").click(function (e) {
	e.preventDefault();
});