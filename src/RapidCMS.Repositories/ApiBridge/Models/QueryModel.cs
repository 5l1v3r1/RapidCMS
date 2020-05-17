﻿using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RapidCMS.Core.Abstractions.Data;
using RapidCMS.Core.Models.Data;
using static RapidCMS.Core.Models.Data.Query;

namespace RapidCMS.Repositories.ApiBridge.Models
{
    public class QueryModel
    {
        public QueryModel()
        {

        }

        public QueryModel(IParent? parent)
        {
            ParentPath = parent?.GetParentPath()?.ToPathString();
        }

        public QueryModel(IParent? parent, IQuery query) : this(parent)
        {
            Skip = query.Skip;
            Take = query.Take;
            SearchTerm = query.SearchTerm;
        }

        public QueryModel(IParent? parent, Type? variantType, IQuery query) : this(parent, query)
        {
        }

        public string? ParentPath { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public string? SearchTerm { get; set; }

        [JsonIgnore]
        public IParent? Parent
        {
            get
            {
                return default;
            }
        }

        [JsonIgnore]
        public Type? VariantType
        {
            get
            {
                return default;
            }
        }

        [JsonIgnore]
        public IQuery Query
        {
            get
            {
                return Create(Take, Skip / Math.Max(1, Take), SearchTerm, 0);
            }
        }
    }
}
