using System;
using System.Collections;
using System.Collections.Generic;

namespace MvuSharp.Testing
{
    public static class MediatorExtensions
    {
        public static void TestMvuFunc<TModel, TMsg>(
            this IMediator mediator,
            (TModel, CommandHandler<TMsg>) result,
            Action<TModel> assertModel = null,
            Action<IEnumerable<TMsg>> assertMsg = null)
        {
            var (model, cmd) = result;
            assertModel?.Invoke(model);
            if (assertMsg != null)
            {
                mediator.TestCommand(cmd, assertMsg);
            }
        }

        public static void TestCommand<TMsg>(
            this IMediator mediator, 
            CommandHandler<TMsg> command, 
            Action<IEnumerable<TMsg>> assertMsg)
        {
            var msgList = new LinkedList<TMsg>();
            command(mediator, msg => msgList.AddLast(msg), default).Wait();
            assertMsg(msgList);
        }

        public static void TestAggregateRequest<THandler, TRequest, TResponse>(
            this IMediator mediator,
            TRequest request,
            Action<TResponse> assertResponse)
            where THandler : AggregateRequestHandler<TRequest, TResponse>
        {
            var handler = Activator.CreateInstance<THandler>();
            assertResponse(handler.HandleAsync(request, default).Result);
        }
    }
}