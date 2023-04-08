// Let's listen for DOM-Loading to be completed and fire an event to let 
// JSInterop know that we're ready to run JSInterop calls.
document.addEventListener("DOMContentLoaded", function() {
	DotNet.invokeMethodAsync("TRMBlazor.WebAssembly", "JsRuntimeReady");
});

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