# Turg App

Web Api service for our e-commerce startup, Turg. This service involves Users, Products and Customer Shopping Carts.

## Setting up on dev env

1. Spin up a new Postgres instance.

```sh
docker run --name postgres-turg -p 5432:5432 -e POSTGRES_PASSWORD=mysecretpassword -d postgres:16.0-alpine3.18
```

2. Execute the sql script in `init.sql`.

3. Run the application.

```sh
dotnet run
```

## Misc

"Turg" means "market" in Estonian language 🇪🇪

