import { defineConfig } from 'orval';

export default defineConfig({
  'events-api-spec-url-transformer': {
    output: {
      mode: 'single',
      target: './src/generated/eventsApi.ts',
      schemas: './src/generated/model',
      client: 'fetch',
      override: {
        mutator: {
          path: './src/customFetch.ts',
          name: 'customFetch',
        },
      },
    },
    input: {
      target: 'http://localhost:5121/swagger/v1/swagger.json',
    },
  },
});