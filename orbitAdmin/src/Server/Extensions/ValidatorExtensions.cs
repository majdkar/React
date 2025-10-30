using System.Linq;
using SchoolV01.Application.Configurations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using SchoolV01.Application.Validators.Features.ExtendedAttributes.Commands;
using SchoolV01.Application.Features.ExtendedAttributes.Commands;

namespace SchoolV01.Server.Extensions
{
    internal static class ValidatorExtensions
    {
        internal static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<AppConfiguration>();
            return services;
        }

        internal static void AddExtendedAttributesValidators(this IServiceCollection services)
        {
            #region AddEditExtendedAttributeCommandValidator

            var addEditExtendedAttributeCommandValidatorType = typeof(AddEditExtendedAttributeCommandValidator<,,,>);
            var validatorTypes = addEditExtendedAttributeCommandValidatorType
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.BaseType?.IsGenericType == true)
                .Select(t => new
                {
                    BaseGenericType = t.BaseType,
                    CurrentType = t
                })
                .Where(t => t.BaseGenericType?.GetGenericTypeDefinition() == typeof(AddEditExtendedAttributeCommandValidator<,,,>))
                .ToList();

            foreach (var validatorType in validatorTypes)
            {
                var addEditExtendedAttributeCommandType = typeof(AddEditExtendedAttributeCommand<,,,>).MakeGenericType(validatorType.BaseGenericType.GetGenericArguments());
                var iValidator = typeof(IValidator<>).MakeGenericType(addEditExtendedAttributeCommandType);
                services.AddScoped(iValidator, validatorType.CurrentType);
            }

            #endregion AddEditExtendedAttributeCommandValidator
        }
    }
}