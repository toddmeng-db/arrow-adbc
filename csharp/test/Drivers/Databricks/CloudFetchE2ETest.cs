﻿/*
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
using System.Threading.Tasks;
using Apache.Arrow.Adbc.Drivers.Databricks;
using Xunit;
using Xunit.Abstractions;

namespace Apache.Arrow.Adbc.Tests.Drivers.Databricks
{
    /// <summary>
    /// End-to-end tests for the CloudFetch feature in the Databricks ADBC driver.
    /// </summary>
    public class CloudFetchE2ETest : TestBase<DatabricksTestConfiguration, DatabricksTestEnvironment>
    {
        public CloudFetchE2ETest(ITestOutputHelper? outputHelper)
            : base(outputHelper, new DatabricksTestEnvironment.Factory())
        {
            // Skip the test if the DATABRICKS_TEST_CONFIG_FILE environment variable is not set
            Skip.IfNot(Utils.CanExecuteTestConfig(TestConfigVariable));
        }

        /// <summary>
        /// Integration test for running a large query against a real Databricks cluster.
        /// </summary>
        [Fact]
        public async Task TestRealDatabricksCloudFetchSmallResultSet()
        {
            await TestRealDatabricksCloudFetchLargeQuery("SELECT * FROM range(1000)", 1000);
        }

        [Fact]
        public async Task TestRealDatabricksCloudFetchLargeResultSet()
        {
            await TestRealDatabricksCloudFetchLargeQuery("SELECT * FROM main.tpcds_sf10_delta.catalog_sales LIMIT 1000000", 1000000);
        }

        [Fact]
        public async Task TestRealDatabricksNoCloudFetchSmallResultSet()
        {
            await TestRealDatabricksCloudFetchLargeQuery("SELECT * FROM range(1000)", 1000, false);
        }

        [Fact]
        public async Task TestRealDatabricksNoCloudFetchLargeResultSet()
        {
            await TestRealDatabricksCloudFetchLargeQuery("SELECT * FROM main.tpcds_sf10_delta.catalog_sales LIMIT 1000000", 1000000, false);
        }

        private async Task TestRealDatabricksCloudFetchLargeQuery(string query, int rowCount, bool useCloudFetch = true)
        {
            // Create a statement with CloudFetch enabled
            var statement = Connection.CreateStatement();
            statement.SetOption(DatabricksParameters.UseCloudFetch, useCloudFetch.ToString());
            statement.SetOption(DatabricksParameters.CanDecompressLz4, "true");
            statement.SetOption(DatabricksParameters.MaxBytesPerFile, "10485760"); // 10MB

            // Execute a query that generates a large result set using range function
            statement.SqlQuery = query;

            // Execute the query and get the result
            var result = await statement.ExecuteQueryAsync();

            if (result.Stream == null)
            {
                throw new InvalidOperationException("Result stream is null");
            }

            // Read all the data and count rows
            long totalRows = 0;
            RecordBatch? batch;
            while ((batch = await result.Stream.ReadNextRecordBatchAsync()) != null)
            {
                totalRows += batch.Length;
            }

            Assert.True(totalRows >= rowCount);

            // Also log to the test output helper if available
            OutputHelper?.WriteLine($"Read {totalRows} rows from range function");
        }
    }
}
