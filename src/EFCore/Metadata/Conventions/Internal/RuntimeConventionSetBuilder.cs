// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal
{
    /// <summary>
    ///     <para>
    ///         This API supports the Entity Framework Core infrastructure and is not intended to be used
    ///         directly from your code. This API may change or be removed in future releases.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Scoped"/> and multiple registrations
    ///         are allowed. This means that each <see cref="DbContext"/> instance will use its own
    ///         set of instances of this service.
    ///         The implementations may depend on other services registered with any lifetime.
    ///         The implementations do not need to be thread-safe.
    ///     </para>
    /// </summary>
    public class RuntimeConventionSetBuilder : IConventionSetBuilder
    {
        private readonly IProviderConventionSetBuilder _conventionSetBuilder;
        private readonly IList<IConventionSetCustomizer> _customizers;

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public RuntimeConventionSetBuilder(
            [NotNull] IProviderConventionSetBuilder providerConventionSetBuilder,
            [NotNull] IEnumerable<IConventionSetCustomizer> customizers)
        {
            _conventionSetBuilder = providerConventionSetBuilder;
            _customizers = customizers.ToList();
        }

        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public virtual ConventionSet CreateConventionSet()
        {
            var conventionSet = _conventionSetBuilder.CreateConventionSet();

            foreach (var customizer in _customizers)
            {
                conventionSet = customizer.ModifyConventions(conventionSet);
            }

            return conventionSet;
        }
    }
}