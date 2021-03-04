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
        /// Arranges a substitute to be returned.
        /// </summary>
        /// <typeparam name="TSubtistute">The type of the subtistute.</typeparam>
        /// <param name="substitute">The substitute.</param>
        /// <returns>A substitute.</returns>
        public static TSubtistute ReturnsSubstitute2<TSubtistute>(this TSubtistute substitute)
            where TSubtistute : class
        {
            var newSubstitute = Substitute.For<TSubtistute>();
            substitute.Returns(newSubstitute);
            return newSubstitute;
        }
    }
}
