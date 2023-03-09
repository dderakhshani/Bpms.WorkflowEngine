﻿using System.Reflection;
using AutoMapper;

namespace Bpms.WorkflowEngine.Infrastructure.Mappings
{
    public static class MappingExtentions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>
        (this IMappingExpression<TSource, TDestination> expression)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var sourceType = typeof(TSource);
            var destinationProperties = typeof(TDestination).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (sourceType.GetProperty(property.Name, flags) == null)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                   // expression.ForAllMembers(x=>x.DoNotAllowNull());
                }
            }
            return expression;
        }
    }
}
