using AutoMapper;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using UserZoom.Domain;
using UserZoom.Domain.TaskManagement;
using UserZoom.Domain.TaskManagement.Specs;
using UserZoom.Shared.Data;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Shared.Patterns.Repository;
using UserZoom.Shared.Patterns.Specification;
using UserZoom.Shared.Patterns.UnitOfWork;
using WebApiHost.Controllers;
using WebApiHost.Dto;
using WebApiHost.Installers;

namespace WebApiHost
{
    public static class Container
    {
        public static IWindsorContainer Configure()
        {
            WindsorContainer container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));
            container.Install
            (
                new AutoMapperInstaller(),
                new ApiControllerInstaller("WebApiHost"),
                new DomainInstaller("UserZoom.Domain")
            );

            container.Register
            (
                // Units of work

                Component.For<UZTaskContext>().LifestyleBoundTo<ApiController>(),

                // Id generators
                Component.For<IIdGenerator<Guid>>()
                        .ImplementedBy<GuidIdGenerator>().LifestyleTransient(),

                // Specs
                Component.For<ISpecification<Guid, UZTask>>()
                         .ImplementedBy<AddOrUpdateTaskSpec>()
                         .LifestyleTransient()

                // Infrastructure
            );

            return container;
        }
    }
}
