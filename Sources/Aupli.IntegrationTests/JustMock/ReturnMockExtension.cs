// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMockExtension.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.JustMock
{
    using Telerik.JustMock;
    using Telerik.JustMock.Expectations;

    public static class ReturnMockExtension
    {
        public static TValue ReturnsMock<TValue>(this FuncExpectation<TValue> funcExpectation)
        {
            var mock = Mock.Create<TValue>();
            funcExpectation.Returns(mock);
            return mock;
        }
    }
}