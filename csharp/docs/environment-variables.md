# Environment Variable Support for ADBC Drivers

ADBC drivers now support configuring parameters via environment variables. This allows you to set configuration options without modifying your code, which is especially useful for:

- Deploying to different environments (development, testing, production)
- Setting sensitive information like credentials without hardcoding them
- Overriding default settings for specific deployments

## How Environment Variables Work

For any parameter that can be passed to an ADBC driver, you can set an equivalent environment variable by:

1. Converting the parameter name to uppercase
2. Replacing dots (`.`) with underscores (`_`)
3. Adding the prefix `ADBC_`

For example:
- Parameter: `adbc.databricks.cloudfetch.enabled`
- Environment variable: `ADBC_DATABRICKS_CLOUDFETCH_ENABLED`

## Priority Order

When determining the value for a parameter, the driver checks in this order:

1. Explicitly provided parameter in code (highest priority)
2. Environment variable
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
    
    // This will override the environment variable
    { "adbc.databricks.cloudfetch.enabled", "true" }
};

var db = new DatabricksDatabase(parameters);
var connection = db.Connect();

// The connection will use:
// - Host from ADBC_DATABRICKS_HOST environment variable
// - Token from ADBC_DATABRICKS_TOKEN environment variable
// - CloudFetch enabled (true) from explicit parameter (overriding environment variable)
```

## Security Considerations

Environment variables are a good way to separate configuration from code, but they may be visible to other processes on the same system. For highly sensitive information like credentials, consider using a secure secret management solution appropriate for your deployment environment.

## Testing with Environment Variables

When testing applications that use environment variables for configuration, remember to:

1. Set the environment variables before running your tests
2. Clean up environment variables after tests complete
3. Be aware that environment variables can affect other tests running in the same process

The ADBC library includes unit tests that demonstrate how to properly test code that uses environment variables for configuration.

## Available Environment Variables

All ADBC parameters can be set via environment variables. Here are some commonly used ones:

### Databricks-specific Parameters

- `ADBC_DATABRICKS_CLOUDFETCH_ENABLED` - Whether to use CloudFetch for retrieving results
- `ADBC_DATABRICKS_CLOUDFETCH_LZ4_ENABLED` - Whether the client can decompress LZ4 compressed results
- `ADBC_DATABRICKS_CLOUDFETCH_MAX_BYTES_PER_FILE` - Maximum bytes per file for CloudFetch
- `ADBC_DATABRICKS_ENABLE_DIRECT_RESULTS` - Whether to enable the use of direct results when executing queries
- `ADBC_DATABRICKS_OAUTH_GRANT_TYPE` - The OAuth grant type to use for authentication
- `ADBC_DATABRICKS_OAUTH_CLIENT_ID` - The OAuth client ID for client credentials flow
- `ADBC_DATABRICKS_OAUTH_CLIENT_SECRET` - The OAuth client secret for client credentials flow

### Spark Parameters

- `ADBC_SPARK_HOST` - The hostname of the Spark server
- `ADBC_SPARK_PORT` - The port number of the Spark server
- `ADBC_SPARK_PATH` - The path component of the Spark server URL
- `ADBC_SPARK_TOKEN` - The authentication token for the Spark server
- `ADBC_SPARK_ACCESS_TOKEN` - The OAuth access token (required when authType is oauth)
- `ADBC_SPARK_AUTH_TYPE` - The authentication type to use (none, username_only, basic, token, oauth)

### Core ADBC Connection Options

- `ADBC_CONNECTION_AUTOCOMMIT` - Whether autocommit is enabled
- `ADBC_CONNECTION_READONLY` - Whether the connection should be read-only
- `ADBC_CONNECTION_TRANSACTION_ISOLATION_LEVEL` - Transaction isolation level
- `ADBC_CONNECTION_CATALOG` - Current catalog
- `ADBC_CONNECTION_DB_SCHEMA` - Current schema

For a complete list of available parameters, refer to the parameter classes in the ADBC driver code.