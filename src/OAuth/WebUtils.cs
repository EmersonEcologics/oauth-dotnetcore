﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace OAuth
{
    public static class WebUtils
    {
        public static IEnumerable<WebParameter> ParseQueryString(Uri uri)
        {
            if (uri is null) { throw new ArgumentNullException(nameof(uri)); }

            NameValueCollection parsedQuery = HttpUtility.ParseQueryString(uri.Query);

            var uriDoesNotHaveValidQueryStringExceptionMessage = "Uri does not have valid query string";
            if (uri.Query.Any() && parsedQuery.Count == 0)
            {
                throw new ArgumentException(uriDoesNotHaveValidQueryStringExceptionMessage);
            }

            var queryStringParameters =
                parsedQuery.AllKeys.SelectMany(parsedQuery.GetValues, (key, value) => new {key, value});

            // ReSharper disable once PossibleMultipleEnumeration
            if (queryStringParameters.Any(queryStringParameter => queryStringParameter.key == null))
            {
                throw new ArgumentException(uriDoesNotHaveValidQueryStringExceptionMessage);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            foreach (var param in queryStringParameters)
            {
                yield return new WebParameter(param.key, param.value);
            }
        }
    }
}