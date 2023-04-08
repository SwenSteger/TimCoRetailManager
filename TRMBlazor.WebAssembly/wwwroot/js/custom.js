window.clearModalErrorMessage = (dotnetHelper) => {
	$('#loginModal').on('show.bs.modal', function () {
		$(this).on('hidden.bs.modal', function () {
			dotnetHelper.invokeMethodAsync('ClearErrorMessage');
			$(this).off('hidden.bs.modal');
		});
	});

	$(document).on('click', function (e) {
		if ($(e.target).hasClass('modal') && $('#loginModal').hasClass('show')) {
			dotnetHelper.invokeMethodAsync('ClearErrorMessage');
		}
	});
};

window.openLoginModal = () => {
	$('#loginModal').modal('show');
}