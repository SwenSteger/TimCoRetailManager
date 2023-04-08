using Microsoft.JSInterop;

namespace TRMBlazor.WebAssembly;

public static class JsInteropEvents
{
	public static event Func<Task> OnJsRuntimeReady;

	[JSInvokable]
	public static void JsRuntimeReady()
	{
		OnJsRuntimeReady?.Invoke();
	}
}