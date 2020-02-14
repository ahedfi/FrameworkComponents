using Ahedfi.Component.Data.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace Ahedfi.Component.Data.Infrastructure.Behaviors
{
    public class TransactionBehavior<T> : DispatchProxyAsync
    {
        private T _impl;
        private IUnitOfWork _unitOfWork;
        public void SetParameters(T decorated, IUnitOfWork unitOfWork)
        {
            _impl = decorated;
            _unitOfWork = unitOfWork;
        }
        private async Task InvokeInternal(MethodInfo targetMethod, object[] args)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
                    if (obj is Task)
                    {
                        var task = (Task) obj;
                        await task;
                        if (task.Exception != null)
                            throw new InvalidOperationException("Invalid operation", task.Exception);
                        else
                        {
                            await _unitOfWork.CommitAsync();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        await _unitOfWork.CommitAsync();
                        scope.Complete();
                    }
                }
                catch (Exception ex)
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

        public static T Create(T decorated, IUnitOfWork unitOfWork)
        {
            object proxy = Create<T, TransactionBehavior<T>>();
            ((TransactionBehavior<T>)proxy).SetParameters(decorated, unitOfWork);
            return (T)proxy;
        }

        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            await InvokeInternal(method, args);
        }

        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
             await InvokeInternal(method, args);
            return await Task.FromResult(default(T));
        }
    }
}
