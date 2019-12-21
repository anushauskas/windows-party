using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using Contracts;
using DataAccesLayer;
using Serilog;
using WinParty.Services;
using WinParty.ViewModels;

namespace WinParty
{
    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer _container;
        public Bootstrapper()
        {
            Initialize();
        }

        
        protected override void Configure()
        {
            _container = new CompositionContainer(new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Environment.CurrentDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Application Starting");

            var batch = new CompositionBatch();
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(new VaultService());
            batch.AddExportedValue<IDataService>(new DataService());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }
        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}