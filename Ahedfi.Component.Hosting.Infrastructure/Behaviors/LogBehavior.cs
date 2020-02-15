using Ahedfi.Component.Hosting.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahedfi.Component.Hosting.Infrastructure.Behaviors
{
    public class LogBehavior<T> : DispatchProxyAsync
    {
        private T _impl;
        public void SetParameters(T decorated)
        {
            _impl = decorated;
        }
        private void WriteLog(string message)
        {
            Console.WriteLine("Profiler: {0}", message);
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
                WriteLog($"Invoking method {targetMethod.Name} at {startTime.ToLongTimeString()}");

                var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);

                var endTime = DateTime.Now;
                var timeSpan = endTime - startTime;

                if (obj is Task resultTask)
                {
                    await resultTask;
                    WriteLog($"Method {targetMethod.Name} at {endTime.ToLongTimeString()}.  Elapsed Time: {timeSpan.TotalMilliseconds} ms");
                }
                else
                {
                    WriteLog($"Method {targetMethod.Name} at {endTime.ToLongTimeString()}.  Elapsed Time: {timeSpan.TotalMilliseconds} ms");
                }
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

        public static T Create(T decorated)
        {
            object proxy = Create<T, LogBehavior<T>>();
            ((LogBehavior<T>)proxy).SetParameters(decorated);
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
