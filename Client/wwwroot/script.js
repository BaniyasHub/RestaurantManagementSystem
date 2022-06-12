
$(document).ready(function () {
	// Progress
	var progressBar = $(".loading-bar span");
	var progressAmount = $(".loading-bar").attr("data-progress");
	progressAmount = 0;

	var loadingDelay = setTimeout(function () {
		var interval = setInterval(function () {
			progressAmount += 10;

			progressBar.css("width", progressAmount + "%");

			if (progressAmount >= 100) {
				setTimeout(function () {
					clearInterval(interval);
					reverseAnimation();
				}, 300);
			}
		}, 300);
	}, 2000);

	// Processing over
	function reverseAnimation() {
		$("#processing").removeClass("uncomplete").addClass("complete");
	}

	// Debug button
	$("#trigger").on("click", function () {
		if ($("#processing.uncomplete").length) {
			$("#processing").removeClass("uncomplete").addClass("complete");
		} else {
			$("#processing").removeClass("complete").addClass("uncomplete");
		}
	});
});
