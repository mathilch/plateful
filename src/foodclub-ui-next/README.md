This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Project structure

`app/` - main application folder. \
`components/core` - project own components. (Add your custom components here) \
`components/ui` - components imported by shadcn(component library). Example: pnpm dlx shadcn@latest add button. Check out docs: https://ui.shadcn.com/docs/components \


Next.Js routing example: 
- route `/createFoodEvent` -> opens a `app/createFoodEvent/page.tsx` page



## Getting Started

### Important - before running the project:

Before running the project add your personal github token to your PC environment variable 'GITHUB_NODE_AUTH_TOKEN'.

The token is used at .npmrc config: npm.pkg.github.com/:_authToken=${GITHUB_NODE_AUTH_TOKEN}

How to generate the token: https://www.theserverside.com/blog/Coffee-Talk-Java-News-Stories-and-Opinions/How-to-create-a-GitHub-Personal-Access-Token-example

### Run the development server

First, run the development server
by pressing F5 in Visual Studio Code (check out .vscode/lauch.json for configurations) \
or by running the following scripts:

```bash
#preferred
pnpm dev 
# or
npm run dev
# or
yarn dev
# or
bun dev
```

Open [http://localhost:3000](http://localhost:3000) with your browser to see the result.

You can start editing the page by modifying `app/page.tsx`. The page auto-updates as you edit the file.

This project uses [`next/font`](https://nextjs.org/docs/app/building-your-application/optimizing/fonts) to automatically optimize and load [Geist](https://vercel.com/font), a new font family for Vercel.

## Learn More

To learn more about Next.js, take a look at the following resources:

- [Next.js Documentation](https://nextjs.org/docs) - learn about Next.js features and API.
- [Learn Next.js](https://nextjs.org/learn) - an interactive Next.js tutorial.

You can check out [the Next.js GitHub repository](https://github.com/vercel/next.js) - your feedback and contributions are welcome!

## Deploy on Vercel

The easiest way to deploy your Next.js app is to use the [Vercel Platform](https://vercel.com/new?utm_medium=default-template&filter=next.js&utm_source=create-next-app&utm_campaign=create-next-app-readme) from the creators of Next.js.

Check out our [Next.js deployment documentation](https://nextjs.org/docs/app/building-your-application/deploying) for more details.

Run backend:
One-time steps:
1. install postgresql (search command)
2. start postgresql (search command)
3. run: psql postgres
    3.1. inside psql: CREATE USER fcuser WITH PASSWORD '12345678';\q

Every time steps:
1. Go to API path (use cd)
2. run: psql -h localhost -U fcuser -d foodclub
3. run: dotnet run --environment Development
4. Open url gived in your browser