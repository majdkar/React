using System.Collections.Generic;
using System.Linq;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SchoolV01.Application.Features.ExtendedAttributes.Commands;
using SchoolV01.Application.Features.ExtendedAttributes.Queries;
using SchoolV01.Application.Features.ExtendedAttributes.QueriesByEntityId;
using SchoolV01.Domain.Contracts;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Application.Localization;

namespace SchoolV01.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });           
            
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddSingleton<LocalizationService>();

        }

        public static void AddExtendedAttributesHandlers(this IServiceCollection services)
        {
            var extendedAttributeTypes = typeof(IEntity)
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true)
                .Select(t => new
                {
                    BaseGenericType = t.BaseType,
                    CurrentType = t
                })
                .Where(t => t.BaseGenericType?.GetGenericTypeDefinition() == typeof(AuditableEntityExtendedAttribute<,,>))
                .ToList();

            foreach (var extendedAttributeType in extendedAttributeTypes)
            {
                var extendedAttributeTypeGenericArguments = extendedAttributeType.BaseGenericType.GetGenericArguments().ToList();
                extendedAttributeTypeGenericArguments.Add(extendedAttributeType.CurrentType);

                #region AddEditExtendedAttributeCommandHandler

                var tRequest = typeof(AddEditExtendedAttributeCommand<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                var tResponse = typeof(Result<>).MakeGenericType(extendedAttributeTypeGenericArguments.First());
                var Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                var implementationType = typeof(AddEditExtendedAttributeCommandHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion AddEditExtendedAttributeCommandHandler

                #region DeleteExtendedAttributeCommandHandler

                tRequest = typeof(DeleteExtendedAttributeCommand<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                tResponse = typeof(Result<>).MakeGenericType(extendedAttributeTypeGenericArguments.First());
                Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                implementationType = typeof(DeleteExtendedAttributeCommandHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion DeleteExtendedAttributeCommandHandler

                #region GetAllExtendedAttributesByEntityIdQueryHandler

                tRequest = typeof(GetAllExtendedAttributesByEntityIdQuery<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                tResponse = typeof(Result<>).MakeGenericType(typeof(List<>).MakeGenericType(
                    typeof(GetAllExtendedAttributesByEntityIdResponse<,>).MakeGenericType(
                        extendedAttributeTypeGenericArguments[0], extendedAttributeTypeGenericArguments[1])));
                Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                implementationType = typeof(GetAllExtendedAttributesByEntityIdQueryHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion GetAllExtendedAttributesByEntityIdQueryHandler

                #region GetExtendedAttributeByIdQueryHandler

                tRequest = typeof(GetExtendedAttributeByIdQuery<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                tResponse = typeof(Result<>).MakeGenericType(
                    typeof(GetExtendedAttributeByIdResponse<,>).MakeGenericType(
                        extendedAttributeTypeGenericArguments[0], extendedAttributeTypeGenericArguments[1]));
                Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                implementationType = typeof(GetExtendedAttributeByIdQueryHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion GetExtendedAttributeByIdQueryHandler

                #region GetAllExtendedAttributesQueryHandler

                tRequest = typeof(GetAllExtendedAttributesQuery<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                tResponse = typeof(Result<>).MakeGenericType(typeof(List<>).MakeGenericType(
                    typeof(GetAllExtendedAttributesResponse<,>).MakeGenericType(
                        extendedAttributeTypeGenericArguments[0], extendedAttributeTypeGenericArguments[1])));
                Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                implementationType = typeof(GetAllExtendedAttributesQueryHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion GetAllExtendedAttributesQueryHandler

                #region ExportExtendedAttributesQueryHandler

                tRequest = typeof(ExportExtendedAttributesQuery<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                tResponse = typeof(Result<>).MakeGenericType(typeof(string));
                Note = typeof(IRequestHandler<,>).MakeGenericType(tRequest, tResponse);
                implementationType = typeof(ExportExtendedAttributesQueryHandler<,,,>).MakeGenericType(extendedAttributeTypeGenericArguments.ToArray());
                services.AddScoped(Note, implementationType);

                #endregion ExportExtendedAttributesQueryHandler
            }
        }
    }
}