How to build sdk node package:

1. run the events service, so it's hosted at http://localhost:5121 or change the config (orval.config.js)
1. run `orval --config .\orval.config.js `
2. Before publishing set your github personal token in the env variable: GITHUB_NODE_AUTH_TOKEN
2. `cd /src`
4. `npm version patch`
3. `npm run build` 
5. `npm publish`