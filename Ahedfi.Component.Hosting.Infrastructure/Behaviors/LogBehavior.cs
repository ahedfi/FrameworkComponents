using Ahedfi.Component.Hosting.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahedfi.Component.Hosting.Infrastructure.Behaviors
{
    public class LogBehavior<T> : DispatchProxyAsync
    {
        private T _impl;
        private ILogger<LogBehavior<T>> _logger;
        public void SetParameters(T decorated, ILogger<LogBehavior<T>> logger)
        {
            _impl = decorated;
            _logger = logger;
        }
        private void LogException(Exception exception, MethodInfo methodInfo = null)
        {
            try
            {
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Class {_impl.GetType().FullName}");
                errorMessage.AppendLine($"Method {methodInfo?.Name} threw exception");
                errorMessage.AppendLine(exception.GetDescription());

            }
            catch (Exception)
            {
                // ignored  
                //Method should return original exception  
            }
        }
        private async Task InvokeInternal(MethodInfo targetMethod, object[] args)
        {
            try
            {
                var startTime = DateTime.Now;
                _logger.LogInformation("Invoking method {0} at {1}", targetMethod.Name, startTime.ToLongTimeString());

                var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);

                var endTime = DateTime.Now;
                var timeSpan = endTime - startTime;

                if (obj is Task resultTask)
                {
                    await resultTask;
                    _logger.LogInformation("Method {0} at {1}. Elapsed Time: {2} ms", targetMethod.Name, endTime.ToLongTimeString(), timeSpan.TotalMilliseconds);
                }
                else
                {
                    _logger.LogInformation("Method {0} at {1}. Elapsed Time: {2} ms", targetMethod.Name, endTime.ToLongTimeString(), timeSpan.TotalMilliseconds);
                }
            }
            catch (Exception ex)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
                throw ex.InnerException ?? ex;
            }
        }

        private async Task<T> InvokeInternal<T>(MethodInfo targetMethod, object[] args)
        {
            try
            {
                var startTime = DateTime.Now;
                _logger.LogInformation("Invoking method {0} at {1}", targetMethod.Name, startTime.ToLongTimeString());

                var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);

                var endTime = DateTime.Now;
                var timeSpan = endTime - startTime;

                if (obj is Task<T> resultTask)
                {
                    var result = await resultTask;
                    _logger.LogInformation("Method {0} at {1}. Elapsed Time: {2} ms", targetMethod.Name, endTime.ToLongTimeString(), timeSpan.TotalMilliseconds);

                    return result;
                }

                throw new InvalidOperationException("Invalid async method");
            }
            catch (Exception ex)
            {
                LogException(ex.InnerException ?? ex, targetMethod);
                throw ex.InnerException ?? ex;
            }
        }
        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            InvokeInternal(targetMethod, args).Wait();
            return new object();
        }

        public static T Create(T decorated, ILogger<LogBehavior<T>> logger)
        {
            object proxy = Create<T, LogBehavior<T>>();
            ((LogBehavior<T>)proxy).SetParameters(decorated, logger);
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
