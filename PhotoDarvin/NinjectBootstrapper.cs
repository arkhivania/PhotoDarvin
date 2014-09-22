using System;
using System.Linq;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using PhotoDarvin.Properties;
using CommonServiceLocator.NinjectAdapter.Unofficial;

namespace PhotoDarvin
{
    public abstract class NinjectBootstrapper : Bootstrapper
    {

        private bool _useDefaultConfiguration = true;

        /// <summary>
        /// Gets the default <see cref="Ninject.IKernel"/> for the application.
        /// </summary>
        /// <value>The default <see cref="Ninject.IKernel"/> instance.</value>

        public IKernel Kernel { get; protected set; }


        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="runWithDefaultConfiguration">If <see langword="true"/>, registers default Composite Application Library services in the container. This is the default behavior.</param>
        public override void Run(bool runWithDefaultConfiguration)
        {
            this._useDefaultConfiguration = runWithDefaultConfiguration;

            this.Logger = this.CreateLogger();
            if (this.Logger == null)
            {
                throw new InvalidOperationException("Null logger");
            }

            this.Logger.Log("Logger created", Category.Debug, Priority.Low);

            this.Logger.Log("Create module catalog", Category.Debug, Priority.Low);
            this.ModuleCatalog = this.CreateModuleCatalog();
            if (this.ModuleCatalog == null)
            {
                throw new InvalidOperationException("Null module catalog");
            }

            this.Logger.Log("Configuring module catalog", Category.Debug, Priority.Low);
            this.ConfigureModuleCatalog();

            this.Logger.Log("Creating Kernel", Category.Debug, Priority.Low);
            this.Kernel = this.CreateKernel();
            if (this.Kernel == null)
            {
                throw new InvalidOperationException("Null kernel");
            }

            this.Logger.Log("Configure Kernel", Category.Debug, Priority.Low);
            this.ConfigureContainer();

            this.Logger.Log("ConfiguringServiceLocatorSingleton", Category.Debug, Priority.Low);
            this.ConfigureServiceLocator();

            this.Logger.Log("ConfiguringRegionAdapters", Category.Debug, Priority.Low);
            this.ConfigureRegionAdapterMappings();

            this.Logger.Log("ConfiguringDefaultRegionBehaviors", Category.Debug, Priority.Low);
            this.ConfigureDefaultRegionBehaviors();

            this.Logger.Log("RegisteringFrameworkExceptionTypes", Category.Debug, Priority.Low);
            this.RegisterFrameworkExceptionTypes();

            this.Logger.Log("CreatingShell", Category.Debug, Priority.Low);
            this.Shell = this.CreateShell();
            if (this.Shell != null)
            {
                this.Logger.Log("SettingTheRegionManager", Category.Debug, Priority.Low);
                RegionManager.SetRegionManager(this.Shell, this.Kernel.Get<IRegionManager>());

                this.Logger.Log("UpdatingRegions", Category.Debug, Priority.Low);
                RegionManager.UpdateRegions();

                this.Logger.Log("InitializingShell", Category.Debug, Priority.Low);
                this.InitializeShell();
            }

            if (this.Kernel.IsRegistered<IModuleManager>())
            {
                this.Logger.Log("InitializingModules", Category.Debug, Priority.Low);
                this.InitializeModules();
                foreach (var m in Kernel.GetModules().OfType<IModule>())
                    m.Initialize();
            }

            

            this.Logger.Log("BootstrapperSequenceCompleted", Category.Debug, Priority.Low);
        }

        /// <summary>
        /// Configures the LocatorProvider for the <see cref="ServiceLocator" />.
        /// </summary>
        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => this.Kernel.Get<IServiceLocator>());
        }

        /// <summary>
        /// Registers in the <see cref="Ninject.IKernel"/> the <see cref="Type"/> of the Exceptions
        /// that are not considered root exceptions by the <see cref="ExceptionExtensions"/>.
        /// </summary>
        protected override void RegisterFrameworkExceptionTypes()
        {
            base.RegisterFrameworkExceptionTypes();

            ExceptionExtensions.RegisterFrameworkExceptionType(
                typeof(Ninject.ActivationException));


        }

        /// <summary>
        /// Configures the <see cref="Ninject.IKernel"/>. May be overwritten in a derived class to add specific
        /// type mappings required by the application.
        /// </summary>
        protected virtual void ConfigureContainer()
        {
            Kernel.Bind<ILoggerFacade>().ToConstant(Logger).InSingletonScope();
            Kernel.Bind<IModuleCatalog>().ToConstant(ModuleCatalog).InSingletonScope();

            if (_useDefaultConfiguration)
            {
                this.Kernel.RegisterTypeIfMissing<IServiceLocator, NinjectServiceLocator>(true);
                this.Kernel.RegisterTypeIfMissing<IModuleInitializer, ModuleInitializer>(true);
                this.Kernel.RegisterTypeIfMissing<IModuleManager, ModuleManager>(true);
                this.Kernel.RegisterTypeIfMissing<RegionAdapterMappings, RegionAdapterMappings>(true);
                this.Kernel.RegisterTypeIfMissing<IRegionManager, RegionManager>(true);
                this.Kernel.RegisterTypeIfMissing<IEventAggregator, EventAggregator>(true);
                this.Kernel.RegisterTypeIfMissing<IRegionViewRegistry, RegionViewRegistry>(true);
                this.Kernel.RegisterTypeIfMissing<IRegionBehaviorFactory, RegionBehaviorFactory>(true);
                this.Kernel.RegisterTypeIfMissing<IRegionNavigationJournalEntry, RegionNavigationJournalEntry>(false);
                this.Kernel.RegisterTypeIfMissing<IRegionNavigationJournal, RegionNavigationJournal>(false);
                this.Kernel.RegisterTypeIfMissing<IRegionNavigationService, RegionNavigationService>(false);
                this.Kernel.RegisterTypeIfMissing<IRegionNavigationContentLoader, RegionNavigationContentLoader>(true);
            }
        }

        /// <summary>
        /// Initializes the modules. May be overwritten in a derived class to use a custom Modules catalogue
        /// </summary>
        protected override void InitializeModules()
        {
            IModuleManager manager;

            try
            {
                manager = this.Kernel.Get<IModuleManager>();
            }
            catch (Ninject.ActivationException ex)
            {
                if (ex.Message.Contains("IModuleCatalog"))
                {
                    throw new InvalidOperationException("NullModuleCatalogException");
                }

                throw;
            }

            manager.Run();
        }

        /// <summary>
        /// Creates the <see cref="Ninject.IKernel"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="Ninject.IKernel"/>.</returns>
        protected virtual IKernel CreateKernel()
        {
            return new StandardKernel();
        }
    }
}
