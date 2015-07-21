﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mvc.JQuery.Datatables.CSharpModel;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace Mvc.JQuery.Datatables
{
    internal static class DataTablesFiltering
    {
        public static IQueryable<T> ApplyFilterAndSort<T>(IQueryable<T> query, ModelProperty[] properties, DataTablesRequest dtRequest)
        {
            query = ApplyTableFilter(query, properties, dtRequest);
            query = ApplyColumnFilter(query, properties, dtRequest);
            query = ApplyOrderBy(query, properties, dtRequest);
            return query;
        }

        private static IQueryable<T> ApplyOrderBy<T>(IQueryable<T> query, ModelProperty[] columns, DataTablesRequest dtParameters)
        {
            string sortString = "";
            for (int i = 0; i < dtParameters.Order.Count(); i++)
            {
                int columnNumber = dtParameters.Order[i].Column;
                string columnName = columns[columnNumber].Name;
                string sortDir = dtParameters.Order[i].Dir;
                if (i != 0)
                    sortString += ", ";
                sortString += columnName + " " + sortDir;
            }
            if (string.IsNullOrWhiteSpace(sortString))
            {
                sortString = columns[0].Name;
            }
            query = query.OrderBy(sortString);
            return query;

        }

        private static IQueryable<T> ApplyColumnFilter<T>(IQueryable<T> query, ModelProperty[] properties, DataTablesRequest dtRequest)
        {
            foreach (var sc in dtRequest.Columns.Where(s => s.Searchable == true && s.Search.Value != "").Select(s => s))
            {
                var searchValue = sc.Search.Value;
                var property = properties.Where(c => c.Name == sc.Data).First();
                var builder = new DataTablesWhereBuilder();
                builder.AddFilter(property, searchValue);
                query = query.Where(string.Join(" or ", builder.DynamicLinqString), builder.DynamicLinqParameters.ToArray());
            }
            return query;
        }

        private static IQueryable<T> ApplyTableFilter<T>(IQueryable<T> query, ModelProperty[] properties, DataTablesRequest dtRequest)
        {
            var builder = new DataTablesWhereBuilder();
            var searchValue = dtRequest.Search.Value;
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                foreach (var sc in dtRequest.Columns.Where(s => s.Searchable == true).Select(s => s))
                {
                    var property = properties.Where(c => c.Name == sc.Data).First();
                    builder.AddFilter(property, searchValue);
                }
                if (builder.DynamicLinqParameters.Any())
                    query = query.Where(string.Join(" or ", builder.DynamicLinqString), builder.DynamicLinqParameters.ToArray());
            }
            return query;
        }

    }
}