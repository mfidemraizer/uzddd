using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using AutoMapper;
using System.Reflection;
using UserZoom.Shared.Patterns.Domain;
using UserZoom.Domain;

namespace WebApiHost.Installers
{
    internal sealed class AutoMapperInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            MapperConfiguration mapperConfig = new MapperConfiguration
            (
                config =>
                {
                    IEnumerable<Type> dtos = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Name.EndsWith("Dto"));
                    IEnumerable<Type> domainObjects = Assembly.GetAssembly(typeof(UZTask)).GetTypes().Where(t => typeof(DomainObject).IsAssignableFrom(t));
                    
                    foreach (Type dto in dtos)
                    {
                        Type mappeableDomainObject = domainObjects.SingleOrDefault(t => dto.Name.Contains(t.Name));

                        if (mappeableDomainObject != null)
                        {
                            config.CreateMap(dto, mappeableDomainObject);
                        }
                        //typeof(IProfileExpression).GetMethod("CreateMap").MakeGenericMethod(new[] { typeof(Type), typeof(Type) })
                        //                            .Invoke(config, new object[] { dto, mappeableDomainObject });
                    }

                    //      config.CreateMap<UZTaskCreationDto, UZTask>()
                    // .ForMember(domainObject => domainObject.Title, options => options.Condition(dto => !string.IsNullOrEmpty(dto.Title)))
                    //.AfterMap
                    //(
                    //     (dto, domainObject) =>
                    //     {

                    //     }
                    // );
                }
            );
            container.Register(Component.For<IMapper>().UsingFactoryMethod((a, b) => mapperConfig.CreateMapper()));
        }
    }
}
