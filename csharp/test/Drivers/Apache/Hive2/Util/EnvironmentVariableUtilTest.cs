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
using Apache.Arrow.Adbc.Drivers.Apache.Hive2.Util;
using Xunit;

namespace Apache.Arrow.Adbc.Tests.Drivers.Apache.Hive2.Util
{
    public class EnvironmentVariableUtilTest : IDisposable
    {
        private readonly Dictionary<string, string> _originalEnvVars = new Dictionary<string, string>();
        
        public EnvironmentVariableUtilTest()
        {
            // Save original environment variables that we'll modify
            SaveEnvironmentVariable("ADBC_DATABRICKS_CLOUDFETCH_ENABLED");
            SaveEnvironmentVariable("ADBC_SPARK_HOST");
            SaveEnvironmentVariable("ADBC_SPARK_TOKEN");
        }

        public void Dispose()
        {
            // Restore original environment variables
            foreach (var kvp in _originalEnvVars)
            {
                if (kvp.Value != null)
                {
                    Environment.SetEnvironmentVariable(kvp.Key, kvp.Value);
                }
                else
                {
                    Environment.SetEnvironmentVariable(kvp.Key, null);
                }
            }
        }
        
        private void SaveEnvironmentVariable(string name)
        {
            _originalEnvVars[name] = Environment.GetEnvironmentVariable(name);
        }
        
        [Fact]
        public void TestEnvironmentVariableConversion()
        {
            string paramName = "adbc.databricks.cloudfetch.enabled";
            string envVarName = "ADBC_DATABRICKS_CLOUDFETCH_ENABLED";
            
            Assert.Equal(envVarName, EnvironmentVariableUtil.ParameterToEnvironmentVariable(paramName));
            Assert.Equal(paramName, EnvironmentVariableUtil.EnvironmentVariableToParameter(envVarName));
        }
        
        [Fact]
        public void TestEnvironmentVariableToParameterThrowsForInvalidPrefix()
        {
            Assert.Throws<ArgumentException>(() => 
                EnvironmentVariableUtil.EnvironmentVariableToParameter("INVALID_PREFIX_VAR"));
        }
        
        [Fact]
        public void TestMergeWithEnvironmentVariables()
        {
            // Set environment variables
            Environment.SetEnvironmentVariable("ADBC_DATABRICKS_CLOUDFETCH_ENABLED", "false");
            Environment.SetEnvironmentVariable("ADBC_SPARK_HOST", "env-host");
            
            // Create explicit properties
            var explicitProps = new Dictionary<string, string>
            {
                { "adbc.databricks.cloudfetch.enabled", "true" },
                { "adbc.spark.token", "explicit-token" }
            };
            
            // Merge properties
            var merged = EnvironmentVariableUtil.MergeWithEnvironmentVariables(explicitProps);
            
            // Verify values
            Assert.Equal("true", merged["adbc.databricks.cloudfetch.enabled"]); // Explicit property takes precedence
            Assert.Equal("env-host", merged["adbc.spark.host"]); // From environment variable
            Assert.Equal("explicit-token", merged["adbc.spark.token"]); // From explicit properties
        }
        
        [Fact]
        public void TestMergeWithEnvironmentVariablesEmptyProperties()
        {
            // Set environment variables
            Environment.SetEnvironmentVariable("ADBC_SPARK_HOST", "env-host");
            Environment.SetEnvironmentVariable("ADBC_SPARK_PORT", "10000");
            
            // Merge with empty properties
            var merged = EnvironmentVariableUtil.MergeWithEnvironmentVariables(new Dictionary<string, string>());
            
            // Verify values
            Assert.Equal("env-host", merged["adbc.spark.host"]);
            Assert.Equal("10000", merged["adbc.spark.port"]);
        }
    }
}