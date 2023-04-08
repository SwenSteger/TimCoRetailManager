using System;

namespace TRMFrontEnd.Library.Exceptions
{
	public class ApiException : Exception
	{
		public ApiException(string message) : base(message) { }
	}
}
