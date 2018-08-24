﻿using AmSpaceClient;
using AmSpaceModels;
using AmSpaceModels.Idp;
using AmSpaceModels.Sap;
using AmSpaceTools.Decorators;
using AmSpaceTools.ModelConverters;
using AmSpaceTools.ViewModels;
using AutoMapper;
using ExcelWorker;
using ExcelWorker.Models;
using log4net;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmSpaceTools.Infrastructure
{
    public static class Services
    {
        private static Container _container;
        private static IConfigurationProvider _mapperConfiguration
        {
            get
            {
                return new MapperConfiguration(cfg => {
                    cfg.CreateMap<CompetencyAction, UpdateAction>().ConvertUsing(new ActionToUpdateConverter());
                    cfg.CreateMap<IDictionary<Competency, List<IdpAction>>, IEnumerable<IdpExcelRow>>().ConvertUsing(new CompetencyActionsToExcelRowConverter());
                    cfg.CreateMap<IEnumerable<SapPersonExcelRow>, IEnumerable<SapUser>>().ConvertUsing(new SapPersonExcelToAmspaceConverter());
                });
            }
        }
        public static Container Container
        {
            get
            {
                return _container;
            }
        }
        static Services()
        {
            _container = new Container(_ => {
                _.For<IAmSpaceClient>().Use<AmSpaceHttpClient>().Singleton();
                _.For<MainWindowViewModel>().Use<MainWindowViewModel>().Singleton();
                _.For<IExcelWorker>().Use<AmSpaceExcelWorker>().Transient();
                _.For<IExcelWorker>().DecorateAllWith<ExcelWorkerDecorator>();
                _.For<ILog>().Use(a => LogManager.GetLogger(typeof(App)));
                _.For<IMapper>().Use(a => new Mapper(_mapperConfiguration)).Transient();
                _.For<IAmSpaceEnvironmentsProvider>().Use<AmSpaceEnvironmentsProvider>().Singleton();
                _.Scan(scanner => {
                    scanner.TheCallingAssembly();
                    scanner.WithDefaultConventions();
                });
            });
        }
    }
}
