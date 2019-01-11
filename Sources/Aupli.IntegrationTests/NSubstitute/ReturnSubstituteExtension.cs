// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnSubstituteExtension.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.NSubstitute
{
    using global::NSubstitute;

    public static class ReturnSubstituteExtension
    {
        /// <summary>
        /// Returnses the substitute.
        /// </summary>
        /// <typeparam name="TSubtistute">The type of the subtistute.</typeparam>
        /// <param name="substitute">The substitute.</param>
        /// <returns></returns>
        public static TSubtistute ReturnsSubstitute<TSubtistute>(this TSubtistute substitute)
            where TSubtistute : class
        {
            var newSubstitute = Substitute.For<TSubtistute>();
            substitute.Returns(newSubstitute);
            return newSubstitute;
        }
    }
}