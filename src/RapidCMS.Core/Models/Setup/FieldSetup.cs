﻿using System;
using System.Threading.Tasks;
using RapidCMS.Core.Abstractions.Metadata;
using RapidCMS.Core.Enums;
using RapidCMS.Core.Models.Config;

namespace RapidCMS.Core.Models.Setup
{
    internal class FieldSetup
    {
        internal FieldSetup(FieldConfig field)
        {
            Index = field.Index;
            Description = field.Description;
            Name = field.Name;
            Property = field.Property;
            Expression = field.Expression;
            OrderByExpression = field.OrderByExpression;
            DefaultOrder = field.DefaultOrder;
            IsVisible = field.IsVisible;
            IsDisabled = field.IsDisabled;
        }

        internal int Index { get; set; }
        
        internal string? Name { get; set; }
        internal string? Description { get; set; }

        internal IPropertyMetadata? Property { get; set; }
        internal IExpressionMetadata? Expression { get; set; }
        internal IPropertyMetadata? OrderByExpression { get; set; }
        internal OrderByType DefaultOrder { get; set; }

        internal Func<object, Task<bool>> IsVisible { get; set; }
        internal Func<object, Task<bool>> IsDisabled { get; set; }
    }
}
