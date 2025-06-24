 Take a look at the Databricks driver.

This is because there is a bug on runtime-side:

    Sometimes TSparkDirectResults.TFetchResultsResp.TRowSet could be non-link responses (ARROW_BASED_SET), but still TSparkDirectResults.TGetResultSetMetadataResp.ResultFormat == URL_BASED_SET

This breaks the current consuming for older dbr, since we rely on ResultFormat to determine which type of reader to use. Can you create some sort of unified reader that unified reading to a single path, so we don't have to worry about this? For example, maybe we can have a single reader that encapsulates both. When ReadNextBatchAsync is called, we check if direct results exist, if not we fetch, then we determine how to read afterwards. Can you implement this, while treating it as a refactor as much as possible (do not try and change too much).


Remember, the fact that ResultFormat could be wrong is the issue here. We will have to account for the scenario where direct results is off, but we're not using cloud fetch - we will have to look at the resulting batch after that. 

Goose has made mistakes before here. Remember, you should not be using ResultFormat, not should you need to rely on DirectResults. You may want to design an implementation of ReadAsync that can consume from DirectResult if needed, then fetch if needed, then perform cloudfetch operations if needed, all in one.

Additionally, we may want to make CloudFetchReader and 
