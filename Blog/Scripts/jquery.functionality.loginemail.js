$("#loginEmail > a").click(function () {
	let main = $("#loginForm div:first");
	let mainSaved = main;
	main.empty();
	$(this).text("Use username to login");
});