using Ahedfi.Component.Data.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Transactions;

namespace Ahedfi.Component.Data.Infrastructure.Behaviors
{
    public class TransactionBehavior<T> : DispatchProxy
    {
        private T _impl;
        private IUnitOfWork _unitOfWork;
        public void SetParameters(T decorated, IUnitOfWork unitOfWork)
        {
            _impl = decorated;
            _unitOfWork = unitOfWork;
        }
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
                    var task = obj as Task;
                    if (task != null)
                    {
                        // TODO : Review asynchrounous task
                        task.Wait();
                        if (task.Exception != null)
                            throw new InvalidOperationException("Invalid operation", task.Exception);
                        else {
                            _unitOfWork.CommitAsync().GetAwaiter().GetResult();
                            scope.Complete();
                        }
                    }
                    else
                    {
                        _unitOfWork.CommitAsync().GetAwaiter().GetResult();
                        scope.Complete();
                    }
                    return obj;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
        public static T Create(T decorated, IUnitOfWork unitOfWork)
        {
            object proxy = Create<T, TransactionBehavior<T>>();
            ((TransactionBehavior<T>)proxy).SetParameters(decorated, unitOfWork);
            return (T)proxy;
        }
    }
}
