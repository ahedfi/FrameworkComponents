using Ahedfi.Component.Core.Domain.Validation.interfaces;
using Ahedfi.Component.Hosting.Infrastructure.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ahedfi.Component.Hosting.Infrastructure.Behaviors
{
    public class RequestValidatorBehavior<T> : DispatchProxyAsync
    {
        private T _impl;
        private IRequestValidator _requestValidator;
        public void SetParameters(T decorated, IRequestValidator requestValidator)
        {
            _impl = decorated;
            _requestValidator = requestValidator;
        }
        private async Task InvokeInternal(MethodInfo targetMethod, object[] args)
        {
            try
            {
                if (args[0] == null)
                    throw new ArgumentNullException("you should not pass a null request.");
                if (_requestValidator.HasRules(args[0]))
                {
                    await _requestValidator.ValidateAsync(args[0]);
                }
                _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<T> InvokeInternal<T>(MethodInfo targetMethod, object[] args)
        {
            try
            {
                if (args[0] == null)
                    throw new ArgumentNullException("you should not pass a null request.");
                if (_requestValidator.HasRules(args[0]))
                {
                    await _requestValidator.ValidateAsync(args[0]);
                }
                var obj = _impl.GetType().GetMethod(targetMethod.Name).Invoke(_impl, args);
                var resultTask = obj as Task<T>;
                return await resultTask;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public override object Invoke(MethodInfo targetMethod, object[] args)
        {
            InvokeInternal(targetMethod, args).Wait();
            return new object();
        }
        public static T Create(T decorated, IRequestValidator requestValidator)
        {
            object proxy = Create<T, RequestValidatorBehavior<T>>();
            ((RequestValidatorBehavior<T>)proxy).SetParameters(decorated, requestValidator);
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
