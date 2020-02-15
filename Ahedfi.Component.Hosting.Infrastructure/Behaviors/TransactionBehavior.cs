using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ahedfi.Component.Hosting.Infrastructure.Behaviors
{
    public class TransactionBehavior<T> : DispatchProxyAsync
    {
        private T _impl;
        public void SetParameters(T decorated)
        {
            _impl = decorated;
        }
        private async Task InvokeInternal(MethodInfo targetMethod, object[] args)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
                    if (obj is Task resultTask)
                    {
                        await resultTask;
                        scope.Complete();
                    }
                    else
                    {
                        scope.Complete();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private async Task<T> InvokeInternal<T>(MethodInfo targetMethod, object[] args)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
                    if (obj is Task<T> resultTask)
                    {
                       var result = await resultTask;
                        scope.Complete();
                        return result;
                    }
                    throw new InvalidOperationException("Invalid async method");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            InvokeInternal(targetMethod, args).Wait();
            return new object();
        }

        public static T Create(T decorated)
        {
            object proxy = Create<T, TransactionBehavior<T>>();
            ((TransactionBehavior<T>)proxy).SetParameters(decorated);
            return (T)proxy;
        }

        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            await InvokeInternal(method, args);
        }

        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            return await InvokeInternal<T>(method, args);
        }
    }
}
