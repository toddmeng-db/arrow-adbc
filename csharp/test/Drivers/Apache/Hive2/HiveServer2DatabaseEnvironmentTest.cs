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
using System.Reflection;
using Apache.Arrow.Adbc.Drivers.Apache.Hive2;
using Apache.Arrow.Adbc.Drivers.Apache.Hive2.Util;
using Xunit;

namespace Apache.Arrow.Adbc.Tests.Drivers.Apache.Hive2
{
    public class HiveServer2DatabaseEnvironmentTest : IDisposable
    {
        private readonly Dictionary<string, string> _originalEnvVars = new Dictionary<string, string>();
        
        public HiveServer2DatabaseEnvironmentTest()
        {
            // Save original environment variables that we'll modify
            SaveEnvironmentVariable("ADBC_SPARK_HOST");
            SaveEnvironmentVariable("ADBC_SPARK_PORT");
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
        public void TestDatabaseUsesEnvironmentVariables()
        {
            // Set environment variables
            Environment.SetEnvironmentVariable("ADBC_SPARK_HOST", "env-host");
            Environment.SetEnvironmentVariable("ADBC_SPARK_PORT", "10000");
            Environment.SetEnvironmentVariable("ADBC_SPARK_TOKEN", "env-token");
            
            // Create database with minimal properties
            var props = new Dictionary<string, string>();
            
            // Create the database
            var db = new HiveServer2Database(props);
            
            // Get the properties field using reflection
            var propertiesField = typeof(HiveServer2Database).GetField("properties", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(propertiesField);
            
            var properties = propertiesField.GetValue(db) as IReadOnlyDictionary<string, string>;
            Assert.NotNull(properties);
            
            // Verify values
            Assert.Equal("env-host", properties["adbc.spark.host"]);
            Assert.Equal("10000", properties["adbc.spark.port"]);
            Assert.Equal("env-token", properties["adbc.spark.token"]);
        }
        
        [Fact]
        public void TestExplicitPropertiesTakePrecedenceOverEnvironmentVariables()
        {
            // Set environment variables
            Environment.SetEnvironmentVariable("ADBC_SPARK_HOST", "env-host");
            Environment.SetEnvironmentVariable("ADBC_SPARK_PORT", "10000");
            
            // Create database with explicit properties
            var props = new Dictionary<string, string>
            {
                { "adbc.spark.host", "explicit-host" },
                { "adbc.spark.token", "explicit-token" }
            };
            
            // Create the database
            var db = new HiveServer2Database(props);
            
            // Get the properties field using reflection
            var propertiesField = typeof(HiveServer2Database).GetField("properties", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(propertiesField);
            
            var properties = propertiesField.GetValue(db) as IReadOnlyDictionary<string, string>;
            Assert.NotNull(properties);
            
            // Verify values
            Assert.Equal("explicit-host", properties["adbc.spark.host"]); // Explicit property takes precedence
            Assert.Equal("10000", properties["adbc.spark.port"]); // From environment variable
            Assert.Equal("explicit-token", properties["adbc.spark.token"]); // From explicit properties
        }
    }
}