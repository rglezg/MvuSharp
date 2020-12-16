using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp.Testing
{
	internal class MockRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
		where TRequest : IRequest<TResponse>
	{
		private readonly Func<TRequest, Task<TResponse>> _handleFunc;
		
		public MockRequestHandler(Func<TRequest, TResponse> handleFunc)
		{
			_handleFunc = (command) => Task.FromResult(handleFunc(command));
		}

		public MockRequestHandler(Func<TRequest, Task<TResponse>> handleFunc)
		{
			_handleFunc = handleFunc;
		}

		public Task<TResponse> HandleAsync(TRequest command, CancellationToken cancellationToken) => 
			_handleFunc(command);
	}
}
