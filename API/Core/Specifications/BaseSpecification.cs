﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using Core.Interfaces.Gateways.Reposytories;

namespace Core.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        protected BaseSpecification()
        {

        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

        public List<string> IncludeStrings { get; } = new List<string>();

        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected virtual void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }
    }
}
