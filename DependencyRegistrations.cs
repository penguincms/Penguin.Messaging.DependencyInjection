using Penguin.DependencyInjection.Abstractions.Interfaces;
using Penguin.DependencyInjection.ServiceProviders;
using Penguin.Messaging.Abstractions.Interfaces;
using Penguin.Messaging.Core;
using Penguin.Reflection;
using System;
using System.Collections.Generic;
using DependencyEngine = Penguin.DependencyInjection.Engine;

namespace Framework.Cms.Web.Mvc.DependencyInjection
{
    /// <summary>
    /// Contains the interface that automatically registers message handler types as Transient with the Dependency Injector
    /// </summary>
    public class DependencyRegistrations : IRegisterDependencies
    {
        #region Methods

        /// <summary>
        /// Registers the dependencies.
        /// </summary>
        public void RegisterDependencies()
        {
            IEnumerable<Type> MessageHandlers = TypeFactory.GetAllImplementations(typeof(IMessageHandler));

            MessageBus.SubscribeAll(MessageHandlers);
            foreach (Type messageHandler in MessageHandlers)
            {
                //We want to register each message handler with the service provider in case
                //there are dependencies that need to be resolved in order to use it
                if (!DependencyEngine.IsRegistered(messageHandler))
                {
                    DependencyEngine.Register(messageHandler, messageHandler, typeof(TransientServiceProvider));
                }
            }
        }

        #endregion Methods
    }
}