using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
	public delegate void DispatchHandler<in TMsg>(TMsg msg);
	
	public delegate Task CommandHandler<out TMsg>(IMediator mediator, DispatchHandler<TMsg> dispatchHandler);	
	
	public static class Command
	{
		public static CommandHandler<TMsg> OfMsg<TMsg>(TMsg msg) =>
			(_, dispatch) => 
			{
				dispatch(msg);
				return Task.CompletedTask;
			};

		public static CommandHandler<TMsg> MapResult<TMsg, TResult>(
			IRequest<TResult> request, 
			Func<TResult, TMsg> mapFunc) =>
			async (mediator, dispatch) =>
			{
				var response = await mediator.SendAsync(request);
				dispatch(mapFunc(response));
			};

		public static CommandHandler<TMsg> OfTry<TMsg, TResult>(
			IRequest<TResult> request,
			Action<TResult> onSuccess,
			Action<Exception> onFailure) =>
			async (mediator, _) =>
			{
				try
				{
					var result = await mediator.SendAsync(request);
					onSuccess(result);
				}
				catch (Exception exception)
				{
					onFailure(exception);
				}
			};
	}
}
