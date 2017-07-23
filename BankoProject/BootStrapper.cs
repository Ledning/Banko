using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Orchestra.Views;
using Debugger = BankoProject.Tools.Debugger;
using System.Reflection;
using BankoProject.Tools;
using System.Collections;
using BankoProject.Models;
using SimpleInjector;
using BankoProject.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace BankoProject
{
  class BootStrapper : BootstrapperBase
  {
    private Container _container;

    public BootStrapper()
    {
      LogManager.GetLog = type => new Debugger(type);
      Initialize();
    }

    protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
    {
      //Tells Caliburn Micro(CM) to display the root view of the specified type. You can have a base view, and then other views up the visual tree....
      //or something like that. Google it i dont remember.
      DisplayRootViewFor<MainWindowViewModel>();
    }

    #region MEF

    /* Managed Extensibility Framework
     * A lot of this is just copy paste code, from http://caliburnmicro.com/documentation/bootstrapper
     * This is used to make whatever we store in our SimpleInjector container available across the application.
     * In this example we have put the windowmanager, EventAggregator and the Data object containing all the data into this container. 
     * The WindowManager manages creation and showing of windows/dialogs/stuff like that. 
     * The EventAggregator is a service that provides us with the ability to publish objects from one entity to another, in a loose fashion. http://caliburnmicro.com/documentation/event-aggregator
     * BingoEvent is a dataobject, that is kept in the container so that all parts of the application might share the same data. 
     * 
     */
    protected override void Configure()
    {
      _container = new Container();

      //Collects values/stuff that has to go in our container
      _container = new Container();
      _container.Register<IWindowManager, WindowManager>();
      _container.RegisterSingleton<IEventAggregator, EventAggregator>();
      _container.RegisterSingleton<BingoEvent, BingoEvent>();
      _container.RegisterSingleton<IDialogCoordinator, DialogCoordinator>();
      _container.Verify();
    }

    protected override object GetInstance(Type service, string key)
    {
      if (service == null)
      {
        var typeName = Assembly.GetExecutingAssembly().DefinedTypes.Where(x => x.Name.Contains(key))
          .Select(x => x.AssemblyQualifiedName).Single();

        service = Type.GetType(typeName);
      }
      return _container.GetInstance(service);
    }

    protected override IEnumerable<object> GetAllInstances(Type service)
    {
      IServiceProvider provider = _container;
      Type collectionType = typeof(IEnumerable<>).MakeGenericType(service);
      var services = (IEnumerable<object>) provider.GetService(collectionType);
      return services ?? Enumerable.Empty<object>();
    }

    protected override void BuildUp(object instance)
    {
      var registration = _container.GetRegistration(instance.GetType(), true);
      registration.Registration.InitializeInstance(instance);
    }

    #endregion
  }
}