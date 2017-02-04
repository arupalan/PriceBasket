﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using PriceBasket.Business.Models;
using PriceBasket.Service.DependencyInjection.Modules;
using log4net;

namespace PriceBasket.Service.DependencyInjection
{
    public static class DependencyInjector
    {
        private static readonly IContainer container;

        private static readonly string[] assembliesToScan =
        {
            "PriceBasket.Service",
            "PriceBasket.Business"
        };

        static DependencyInjector()
        {
            var builder = new ContainerBuilder();

            ScanAssemblies(builder);
            RegisterModules(builder);
            RegisterOddBalls(builder);
            container = builder.Build();
        }

        private static void ScanAssemblies(ContainerBuilder builder)
        {
            var assemblies = assembliesToScan
                .Select(Assembly.Load)
                .ToArray();

            //Scan Assemblies for auto registration from Price Basket assemblies only
            builder.RegisterAssemblyTypes(assemblies)
                .AsImplementedInterfaces();
        }

        private static void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<LoggingModule>();
        }


        private static void RegisterOddBalls(ContainerBuilder builder)
        {
            builder.RegisterInstance(LogManager.GetLogger("PriceBasket.Service")).As<ILog>();
            builder.RegisterType<BasketEconomicsManager>()
                .As<IBasketEconomicsManager>()
                .SingleInstance();

        }

        public static T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
