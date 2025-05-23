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
using Apache.Arrow.Adbc.Drivers.Apache.Hive2;

namespace Apache.Arrow.Adbc.Tests.Drivers.Apache.Common
{
    public abstract class CommonTestEnvironment<TConfig> : TestEnvironment<TConfig>
        where TConfig : TestConfiguration
    {
        public CommonTestEnvironment(Func<AdbcConnection> getConnection)
            : base(getConnection)
        {
        }

        internal DataTypeConversion DataTypeConversion => ((HiveServer2Connection)Connection).DataTypeConversion;

        public string? GetValueForProtocolVersion(string? unconvertedValue, string? convertedValue) =>
            ((HiveServer2Connection)Connection).DataTypeConversion.HasFlag(DataTypeConversion.None) ? unconvertedValue : convertedValue;

        public object? GetValueForProtocolVersion(object? unconvertedValue, object? convertedValue) =>
            ((HiveServer2Connection)Connection).DataTypeConversion.HasFlag(DataTypeConversion.None) ? unconvertedValue : convertedValue;

    }
}
