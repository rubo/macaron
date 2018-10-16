// Copyright 2018 Ruben Buniatyan
// Licensed under the MIT License. For full terms, see LICENSE in the project root.

using System;
using Java.Util;

namespace Macaron.Utilities
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Converts the current <see cref="Date"/> object to its equivalent <see cref="DateTime"/> representation.
        /// </summary>
        /// <param name="date">An object that represents the value to convert.</param>
        /// <returns>
        /// An object that represents the value of the current <see cref="Date"/> object converted to <see cref="DateTime"/>.
        /// </returns>
        public static DateTime ToDateTime(this Date date) => DateTimeOffset.FromUnixTimeMilliseconds(date.Time).DateTime;

        /// <summary>
        /// Converts the current <see cref="DateTime"/> object to its equivalent <see cref="Date"/> representation.
        /// </summary>
        /// <param name="dateTime">An object that represents the value to convert.</param>
        /// <returns>
        /// An object that represents the value of the current <see cref="DateTime"/> object converted to <see cref="Date"/>.
        /// </returns>
        public static Date ToJavaDate(this DateTime dateTime) => new Date(new DateTimeOffset(dateTime).ToUnixTimeMilliseconds());
    }
}
