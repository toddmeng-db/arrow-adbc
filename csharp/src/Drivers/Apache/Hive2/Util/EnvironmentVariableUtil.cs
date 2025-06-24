/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *    http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Apache.Arrow.Adbc.Drivers.Apache.Hive2.Util
{
    /// <summary>
    /// Utility class for handling environment variables in ADBC drivers.
    /// </summary>
    internal static class EnvironmentVariableUtil
    {
        private const string EnvVarPrefix = "ADBC_";

        /// <summary>
        /// Merges explicit properties with environment variables, giving precedence to explicit properties.
        /// </summary>
        /// <param name="explicitProperties">The explicitly provided properties.</param>
        /// <returns>A merged dictionary with values from both sources.</returns>
        public static Dictionary<string, string> MergeWithEnvironmentVariables(
            IReadOnlyDictionary<string, string> explicitProperties)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            // First add environment variables
            foreach (var envVar in Environment.GetEnvironmentVariables().Keys.Cast<string>())
            {
                if (envVar.StartsWith(EnvVarPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    string paramName = EnvironmentVariableToParameter(envVar);
                    string? value = Environment.GetEnvironmentVariable(envVar);
                    
                    if (value != null)
                    {
                        result[paramName] = value;
                    }
                }
            }
            
            // Then add explicit properties (overriding env vars if needed)
            foreach (var kvp in explicitProperties)
            {
                result[kvp.Key] = kvp.Value;
            }
            
            return result;
        }

        /// <summary>
        /// Converts an environment variable name to its corresponding ADBC parameter name.
        /// </summary>
        /// <param name="envVarName">The environment variable name (e.g., "ADBC_DATABRICKS_CLOUDFETCH_ENABLED")</param>
        /// <returns>The ADBC parameter name (e.g., "adbc.databricks.cloudfetch.enabled")</returns>
        public static string EnvironmentVariableToParameter(string envVarName)
        {
            if (!envVarName.StartsWith(EnvVarPrefix, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"Environment variable name must start with '{EnvVarPrefix}'", nameof(envVarName));
            }

            return envVarName.Substring(EnvVarPrefix.Length).ToLowerInvariant().Replace('_', '.');
        }

        /// <summary>
        /// Converts an ADBC parameter name to its corresponding environment variable name.
        /// </summary>
        /// <param name="parameterName">The ADBC parameter name (e.g., "adbc.databricks.cloudfetch.enabled")</param>
        /// <returns>The environment variable name (e.g., "ADBC_DATABRICKS_CLOUDFETCH_ENABLED")</returns>
        public static string ParameterToEnvironmentVariable(string parameterName)
        {
            return EnvVarPrefix + parameterName.ToUpperInvariant().Replace('.', '_');
        }
    }
}