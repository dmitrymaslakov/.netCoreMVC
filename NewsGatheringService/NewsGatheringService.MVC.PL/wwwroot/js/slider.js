// With JQuery
/*$("#ex7").slider();

$("#ex7-enabled").click(function () {
	if (this.checked) {
		// With JQuery
		$("#ex7").slider("enable");
		$("#apply").prop('disabled', false);;
	}
	else {
		// With JQuery
		$("#ex7").slider("disable");
		$("#apply").prop('disabled', true);;

	}
});*/
$('#ex7').slider({
	formatter: function (value) {
		return 'Current value: ' + value;
	}
});

$("#apply").click(function () {
	let rateValue = $("#ex7").slider('getValue');
	location.href = `?rate=${rateValue}`
});
