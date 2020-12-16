using System;
using System.Threading;
using System.Threading.Tasks;

namespace MvuSharp
{
	public delegate void DispatchHandler<in TMsg>(TMsg msg);
	
	public delegate Task CommandHandler<out TMsg>(IMediator mediator, DispatchHandler<TMsg> dispatchHandler, CancellationToken cancellationToken);	
	
	public static class Command<TMsg>
	{
		public static readonly CommandHandler<TMsg> None = 
			(manager, dispatch, cancellationToken) 
			=> Task.CompletedTask;
		
		public static CommandHandler<TMsg> OfMsg(TMsg msg) =>
			(manager, dispatch, token) => 
			{
				dispatch(msg);
				return Task.CompletedTask;
			};

		public static CommandHandler<TMsg> MapResult<TResult>(
			IRequest<TResult> request, 
			Func<TResult, TMsg> mapFunc) =>
			async (manager, dispatch, token) =>
			{
				var response = await manager.SendAsync(request, token);
				dispatch(mapFunc(response));
			};

		public static CommandHandler<TMsg> OfTry<TResult>(
			IRequest<TResult> request,
			Action<TResult> onSuccess,
			Action<Exception> onFailure) =>
			async (manager, dispatch, token) =>
			{
				try
				{
					var result = await manager.SendAsync(request, token);
					onSuccess(result);
				}
				catch (Exception exception)
				{
					onFailure(exception);
				}
			};
	}
}
