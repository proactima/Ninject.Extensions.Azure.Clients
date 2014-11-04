﻿using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Ninject.Modules;

namespace Ninject.Extensions.Azure.Clients
{
    /// <summary>
    /// Module that sets up all the bindings the extension needs.
    /// It will only bind types that have not yet been bound. Recommend that you load it last.
    /// </summary>
    public class AzureClientsModule : NinjectModule
    {
        private readonly string _servicebusConnection =
            CloudConfigurationManager.GetSetting("ServiceBusConnectionString");

        private readonly string _storageConnection =
            CloudConfigurationManager.GetSetting("StorageConnectionString");

        public override void Load()
        {
            this.BindUnlessBoundAsSingleton(c =>
            {
                var storageAccount = CloudStorageAccount.Parse(_storageConnection);
                return storageAccount;
            });

            this.BindUnlessBoundAsSingleton(c =>
            {
                var storageAccount = Kernel.Get<CloudStorageAccount>();
                var queueClient = storageAccount.CreateCloudQueueClient();
                return queueClient;
            });

            this.BindUnlessBoundAsSingleton(c =>
            {
                var namespaceManager = NamespaceManager.CreateFromConnectionString(_servicebusConnection);
                return namespaceManager;
            });

            this.BindUnlessBoundAsSingleton(c =>
            {
                var fac = MessagingFactory.CreateFromConnectionString(_servicebusConnection);
                return fac;
            });
        }
    }
}