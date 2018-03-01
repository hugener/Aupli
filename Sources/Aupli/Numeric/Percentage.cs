// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Percentage.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Numeric
{
    using System;
    using Sundew.Base.Equality;

    /// <summary>
    /// Represents a percentage.
    /// </summary>
    /// <seealso cref="IEquatable{Percentage}" />
    public struct Percentage : IEquatable<Percentage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Percentage"/> struct.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        public Percentage(double percentage)
        {
            this.Value = percentage;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Percentage operator +(Percentage lhs, double rhs)
        {
            return new Percentage(lhs.Value + rhs);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Percentage operator -(Percentage lhs, double rhs)
        {
            return new Percentage(lhs.Value - rhs);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Percentage operator *(Percentage lhs, double rhs)
        {
            return new Percentage(lhs.Value * rhs);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Percentage operator /(Percentage lhs, double rhs)
        {
            return new Percentage(lhs.Value / rhs);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static double operator *(double lhs, Percentage rhs)
        {
            return lhs * rhs.Value;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Percentage lhs, Percentage rhs)
        {
            return lhs.Value.Equals(rhs.Value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Percentage lhs, Percentage rhs)
        {
            return !lhs.Value.Equals(rhs.Value);
        }

        /// <summary>
        /// Limits the specified minimum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>The limited percentage.</returns>
        public Percentage Limit(double min, double max)
        {
            return new Percentage(Math.Min(Math.Max(min, this.Value), max));
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.
        /// </returns>
        public bool Equals(Percentage other)
        {
            return this.Value.Equals(other.Value);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return EqualityHelper.Equals(this, obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Value.ToString("P");
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="percentDecimalDigits">The percent decimal digits.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(int percentDecimalDigits)
        {
            return this.Value.ToString($"P{percentDecimalDigits}");
        }
    }
}