This feature targets the databricks driver in csharp. The goal is to make all databricks driver parameters configurable via environment variables. The implementation is now complete.

Below is a list of all configurable variables that affect the Databricks driver:

# Configurable Parameters for Databricks Driver in C#

## Databricks-specific Parameters

These parameters are defined in `DatabricksParameters.cs`:

1. `adbc.databricks.cloudfetch.enabled` - Whether to use CloudFetch for retrieving results. Default: true
2. `adbc.databricks.cloudfetch.lz4.enabled` - Whether the client can decompress LZ4 compressed results. Default: true
3. `adbc.databricks.cloudfetch.max_bytes_per_file` - Maximum bytes per file for CloudFetch. Default: 20MB
4. `adbc.databricks.cloudfetch.max_retries` - Maximum number of retry attempts for CloudFetch downloads. Default: 3
5. `adbc.databricks.cloudfetch.retry_delay_ms` - Delay in milliseconds between CloudFetch retry attempts. Default: 500ms
6. `adbc.databricks.cloudfetch.timeout_minutes` - Timeout in minutes for CloudFetch HTTP operations. Default: 5 minutes
7. `adbc.databricks.cloudfetch.url_expiration_buffer_seconds` - Buffer time in seconds before URL expiration to trigger refresh. Default: 60 seconds
8. `adbc.databricks.cloudfetch.max_url_refresh_attempts` - Maximum number of URL refresh attempts for CloudFetch downloads. Default: 3
9. `adbc.databricks.enable_direct_results` - Whether to enable the use of direct results when executing queries. Default: true
10. `adbc.databricks.apply_ssp_with_queries` - Whether to apply service side properties (SSP) with queries. Default: false
11. `adbc.databricks.ssp_` - Prefix for server-side properties. Properties with this prefix will be passed to the server.
12. `adbc.spark.temporarily_unavailable_retry` - Controls whether to retry requests that receive a 503 response with a Retry-After header. Default: true
13. `adbc.spark.temporarily_unavailable_retry_timeout` - Maximum total time in seconds to retry 503 responses before failing. Default: 900 seconds (15 minutes)
14. `adbc.databricks.cloudfetch.parallel_downloads` - Maximum number of parallel downloads for CloudFetch operations. Default: 3
15. `adbc.databricks.cloudfetch.prefetch_count` - Number of files to prefetch in CloudFetch operations. Default: 2
16. `adbc.databricks.cloudfetch.memory_buffer_size_mb` - Maximum memory buffer size in MB for CloudFetch prefetched files. Default: 200MB
17. `adbc.databricks.cloudfetch.prefetch_enabled` - Whether CloudFetch prefetch functionality is enabled. Default: true
18. `adbc.databricks.oauth.grant_type` - The OAuth grant type to use for authentication. Default: "access_token"
19. `adbc.databricks.oauth.client_id` - The OAuth client ID for client credentials flow.
20. `adbc.databricks.oauth.client_secret` - The OAuth client secret for client credentials flow.
21. `adbc.databricks.oauth.scope` - The OAuth scope for client credentials flow. Default: "sql"
22. `adbc.databricks.enable_multiple_catalog_support` - Whether to use multiple catalogs. Default: true
23. `adbc.databricks.enable_pk_fk` - Whether to enable primary key foreign key metadata call. Default: true

## Inherited from SparkParameters

These parameters are inherited from the parent class `SparkParameters.cs`:

1. `adbc.spark.host` - The hostname of the Spark server
2. `adbc.spark.port` - The port number of the Spark server
3. `adbc.spark.path` - The path component of the Spark server URL
4. `adbc.spark.token` - The authentication token for the Spark server
5. `adbc.spark.access_token` - The OAuth access token (required when authType is oauth)
6. `adbc.spark.auth_type` - The authentication type to use (none, username_only, basic, token, oauth)
7. `adbc.spark.type` - The type of Spark server (http, standard)
8. `adbc.spark.data_type_conv` - Data type conversion settings
9. `adbc.spark.connect_timeout_ms` - Connection timeout in milliseconds
10. `adbc.spark.user_agent_entry` - User agent entry for HTTP requests

## Inherited from ApacheParameters

These parameters are inherited from the `ApacheParameters.cs` class:

1. `adbc.apache.statement.polltime_ms` - Poll time in milliseconds for statement execution
2. `adbc.apache.statement.batch_size` - Batch size for statement execution
3. `adbc.apache.statement.query_timeout_s` - Query timeout in seconds
4. `adbc.apache.statement.is_metadata_command` - Indicator whether to execute a metadata command query
5. `adbc.get_metadata.target_catalog` - Catalog name for metadata commands
6. `adbc.get_metadata.target_db_schema` - Schema name for metadata commands
7. `adbc.get_metadata.target_table` - Table name for metadata commands
8. `adbc.get_metadata.target_table_types` - Table types for GetTables metadata command
9. `adbc.get_metadata.target_column` - Column name for GetColumns metadata command
10. `adbc.get_metadata.foreign_target_catalog` - Foreign catalog name for GetCrossReference metadata command
11. `adbc.get_metadata.foreign_target_db_schema` - Foreign schema name for GetCrossReference metadata command
12. `adbc.get_metadata.foreign_target_table` - Foreign table name for GetCrossReference metadata command
13. `adbc.get_metadata.escape_pattern_wildcards` - Whether to escape pattern wildcard characters. Default: false

## HTTP and TLS Options from HiveServer2Parameters

These parameters are defined in `HiveServer2Parameters.cs` and may apply to Databricks connections:

1. `adbc.http_options.tls.enabled` - Whether TLS is enabled for HTTP connections
2. `adbc.http_options.tls.allow_self_signed` - Whether to allow self-signed certificates
3. `adbc.http_options.tls.allow_hostname_mismatch` - Whether to allow hostname mismatch in certificates
4. `adbc.http_options.tls.trusted_certificate_path` - Path to trusted certificates
5. `adbc.http_options.tls.disable_server_certificate_validation` - Whether to disable server certificate validation

## Proxy Options from HiveServer2Parameters

These parameters are defined in `HiveServer2Parameters.cs` and may apply to Databricks connections:

1. `adbc.proxy_options.use_proxy` - Whether to use a proxy for HTTP connections. Default: false
2. `adbc.proxy_options.proxy_host` - Hostname or IP address of the proxy server
3. `adbc.proxy_options.proxy_port` - Port number of the proxy server
4. `adbc.proxy_options.proxy_ignore_list` - Comma-separated list of hosts or domains that should bypass the proxy
5. `adbc.proxy_options.proxy_auth` - Whether to enable proxy authentication. Default: false
6. `adbc.proxy_options.proxy_uid` - Username for proxy authentication
7. `adbc.proxy_options.proxy_pwd` - Password for proxy authentication

## Core ADBC Connection Options

These parameters are defined in `AdbcOptions.cs` and apply to all ADBC connections:

1. `adbc.connection.autocommit` - Whether autocommit is enabled
2. `adbc.connection.readonly` - Whether the connection should be read-only
3. `adbc.connection.transaction.isolation_level` - Transaction isolation level
4. `adbc.connection.catalog` - Current catalog
5. `adbc.connection.db_schema` - Current schema

## Core ADBC Statement Options

These parameters are defined in `AdbcOptions.cs` and apply to all ADBC statements:

1. `adbc.statement.exec.incremental` - Whether query execution is non-blocking. Default: disabled
2. `adbc.statement.exec.progress` - Progress of a query
3. `adbc.statement.exec.max_progress` - Maximum progress of a query

# Environment Variable Support for Databricks Driver

The Databricks driver now supports configuring all parameters via environment variables. This allows you to set configuration options without modifying your code, which is especially useful for:

- Deploying to different environments (development, testing, production)
- Setting sensitive information like credentials without hardcoding them
- Overriding default settings for specific deployments

## How Environment Variables Work

For any parameter that can be passed to the Databricks driver, you can set an equivalent environment variable by:

1. Converting the parameter name to uppercase
2. Replacing dots (`.`) with underscores (`_`)
3. Adding the prefix `ADBC_`

For example:
- Parameter: `adbc.databricks.cloudfetch.enabled`
- Environment variable: `ADBC_DATABRICKS_CLOUDFETCH_ENABLED`

## Priority Order

When determining the value for a parameter, the driver checks in this order:

1. Environment variable (highest priority)
2. Explicitly provided parameter in code
3. Default value (lowest priority)

## Example Usage

```csharp
// Set environment variables (in your system or deployment environment)
// ADBC_DATABRICKS_HOST=my-workspace.cloud.databricks.com
// ADBC_DATABRICKS_TOKEN=dapi1234567890abcdef
// ADBC_DATABRICKS_CLOUDFETCH_ENABLED=false

// Create connection with minimal parameters
var parameters = new Dictionary<string, string>
{
    // These would normally be required, but can be provided via environment variables
    // { "adbc.spark.host", "my-workspace.cloud.databricks.com" },
    // { "adbc.spark.token", "dapi1234567890abcdef" },
    
    // This will be overridden by the environment variable
    { "adbc.databricks.cloudfetch.enabled", "true" }
};

var db = new DatabricksDatabase(parameters);
var connection = db.Connect();

// The connection will use:
// - Host from ADBC_DATABRICKS_HOST environment variable
// - Token from ADBC_DATABRICKS_TOKEN environment variable
// - CloudFetch disabled (false) from ADBC_DATABRICKS_CLOUDFETCH_ENABLED environment variable
```

## Security Considerations

Environment variables are a good way to separate configuration from code, but they may be visible to other processes on the same system. For highly sensitive information like credentials, consider using a secure secret management solution appropriate for your deployment environment.

## Testing Environment Variables

When testing applications that use environment variables for configuration, remember to:

1. Set the environment variables before running your tests
2. Clean up environment variables after tests complete
3. Be aware that environment variables can affect other tests running in the same process

# Implementation Details

The environment variable support is implemented using a utility class and integrated into the HiveServer2Database class:

1. **EnvironmentVariableUtil**: A utility class that handles the conversion between environment variables and ADBC parameters, and merges environment variables with explicit properties.

2. **Environment Variable Naming**: Environment variables are mapped to ADBC parameters using a consistent naming convention:
   - Convert parameter name to uppercase
   - Replace dots (.) with underscores (_)
   - Add the prefix "ADBC_"

3. **Priority Order**: When determining the value for a parameter, the driver checks in this order:
   - Explicitly provided parameter in code (highest priority)
   - Environment variable
   - Default value (lowest priority)

4. **HiveServer2Database Integration**: The `HiveServer2Database` class has been updated to use the utility class to merge environment variables with explicit properties.

5. **Driver-Specific Implementation**: The Databricks driver inherits from the HiveServer2Database class, so it automatically benefits from the environment variable support.

For more detailed information, see the [Environment Variables Documentation](/home/todd.meng/arrow-adbc/csharp/docs/environment-variables.md).

# Implementation Plan

We've implemented a lightweight approach to support environment variables in the Databricks driver by creating a utility class (`EnvironmentVariableUtil`) and integrating it with the HiveServer2Database class. This approach is simple, efficient, and meets all the requirements while maintaining good separation of concerns.

## Key Benefits

1. **Separation of Concerns**: Environment variable handling is isolated in a dedicated utility class
2. **Reusable Utility**: The utility class can be used by other components if needed
3. **Consistent Interface**: Environment variables follow a predictable naming convention
4. **Minimal Changes**: Changes to existing code are focused and non-disruptive
5. **Automatic Inheritance**: Databricks driver automatically benefits from the implementation

## Example Usage

```csharp
// Set environment variables
// ADBC_DATABRICKS_HOST=my-workspace.cloud.databricks.com
// ADBC_DATABRICKS_TOKEN=dapi1234567890abcdef
// ADBC_DATABRICKS_CLOUDFETCH_ENABLED=false

// Create connection with minimal parameters
var parameters = new Dictionary<string, string>
{
    // These would normally be required, but can be provided via environment variables
    // { "adbc.spark.host", "my-workspace.cloud.databricks.com" },
    // { "adbc.spark.token", "dapi1234567890abcdef" },
    
    // This will override the environment variable
    { "adbc.databricks.cloudfetch.enabled", "true" }
};

// Create the database and connection
var db = new DatabricksDatabase(parameters);
var connection = db.Connect();

// The connection will use:
// - Host from ADBC_DATABRICKS_HOST environment variable
// - Token from ADBC_DATABRICKS_TOKEN environment variable
// - CloudFetch enabled (true) from explicit parameter (overriding environment variable)
```

The Databricks driver includes unit tests that demonstrate how to properly test code that uses environment variables for configuration.