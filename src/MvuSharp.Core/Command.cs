using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
	public delegate void DispatchHandler<in TMsg>(TMsg msg);
	
	public delegate Task CommandHandler<out TMsg>(IMediator mediator, DispatchHandler<TMsg> dispatchHandler, CancellationToken cancellationToken);	
	
	public static class Command
	{
		public static CommandHandler<TMsg> OfMsg<TMsg>(TMsg msg) =>
			(_, dispatch, _) => 
			{
				dispatch(msg);
				return Task.CompletedTask;
			};

		public static CommandHandler<TMsg> MapResult<TMsg, TResult>(
			IRequest<TResult> request, 
			Func<TResult, TMsg> mapFunc) =>
			async (mediator, dispatch, token) =>
			{
				var response = await mediator.SendAsync(request, token);
				dispatch(mapFunc(response));
			};

		public static CommandHandler<TMsg> OfTry<TMsg, TResult>(
			IRequest<TResult> request,
			Action<TResult> onSuccess,
			Action<Exception> onFailure) =>
			async (mediator, _, token) =>
			{
				try
				{
					var result = await mediator.SendAsync(request, token);
					onSuccess(result);
				}
				catch (Exception exception)
				{
					onFailure(exception);
				}
			};
	}
}
