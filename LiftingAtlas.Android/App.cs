using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Autofac;
using LiftingAtlas.Standard;

namespace LiftingAtlas.Droid
{
    [Application]
    public class App : Application
    {
        private static IContainer container;

        public App(
            IntPtr javaReference,
            JniHandleOwnership transfer
            ) : base(
                javaReference,
                transfer
                )
        {

        }

        public static IContainer Container
        {
            get
            {
                return container;
            }
            private set
            {
                container = value;
            }
        }

        public override void OnCreate()
        {
            base.OnCreate();

            ContainerBuilder containerBuilder = new ContainerBuilder();

            containerBuilder
                .RegisterType<HexFirstCharLetterGuidFormatter>()
                .As<IGuidFormatter>()
                .SingleInstance()
                .AutoActivate();

            containerBuilder
                .RegisterType<FormattableGuidProvider>()
                .As<IGuidProvider>()
                .SingleInstance()
                .AutoActivate();

            string personalFolderPath =
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string connectionString =
                $"{Path.Combine(personalFolderPath, "LiftingAtlas.db")}";
            containerBuilder
                .RegisterType<SQLitePlannedCycleRepository>()
                .As<IPlannedCycleRepository>()
                .SingleInstance()
                .AutoActivate()
                .WithParameter("connectionString", connectionString);

            containerBuilder
                .RegisterType<XMLStreamBasedTemplateCycleProvider>()
                .As<IStreamBasedTemplateCycleProvider>()
                .SingleInstance()
                .AutoActivate();

            containerBuilder
                .RegisterType<AssetStreamBasedTemplateCycleProviderMaster>()
                .As<ITemplateCycleProviderMaster>()
                .SingleInstance()
                .AutoActivate()
                .WithParameter("context", this)
                .WithParameter("path", Path.Combine("TemplateCycles", "XML"));

            containerBuilder
                .RegisterType<NearestMultipleProviderFactory>()
                .As<IUniformQuantizationProviderFactory>()
                .SingleInstance()
                .AutoActivate();

            containerBuilder
                .RegisterType<NewPlannedCyclePresenter>()
                .As<INewPlannedCyclePresenter>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<CurrentPlannedCyclePresenter>()
                .As<ICurrentPlannedCyclePresenter>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<PlannedSessionPresenter>()
                .As<IPlannedSessionPresenter>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<PlannedSetPresenter>()
                .As<IPlannedSetPresenter>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<TemplateCyclePresenter>()
                .As<ITemplateCyclePresenter>()
                .InstancePerLifetimeScope();

            containerBuilder
                .RegisterType<TemplateCyclesPresenter>()
                .As<ITemplateCyclesPresenter>()
                .InstancePerLifetimeScope();

            Container = containerBuilder.Build();
        }
    }
}