// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Logging
{
    using System.Globalization;
    using System.Text;

    /// <summary>Extends the string class with easy to use methods.</summary>
    public static class StringExtensions
    {
        private const char SpaceCharacter = ' ';

        /// <summary>  Takes a camel case string and returns a sentence case string.</summary>
        /// <param name="value">The value.</param>
        /// <returns>The sentence case string.</returns>
        public static string FromCamelCaseToSentenceCase(this string value)
        {
            return value.FromCamelCaseToSentenceCase(CultureInfo.CurrentCulture);
        }

        /// <summary>  Takes a camel case string and returns a sentence case string.</summary>
        /// <param name="value">The value.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns>The sentence case string.</returns>
        public static string FromCamelCaseToSentenceCase(this string value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(value[0]);
            for (var index = 1; index < value.Length; index++)
            {
                var character = value[index];
                if (char.IsUpper(character))
                {
                    stringBuilder.Append(SpaceCharacter);
                    stringBuilder.Append(char.ToLower(character, cultureInfo));
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString();
        }
    }
}