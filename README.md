# Optix Tech Test (C# backend API)

Solution for the Optix tech test specification for a C# movies API

## Tech Stack

The solution makes use of the following technologies

- ASP.NET Core minimal APIs
- EF Core
- Npgsql/Postgres support for EF Core
- CsvHelper for reading the supplied data file
- Bogus for creating fake data where needed
- FluentValidation to validate the search input
- xUnit for unit testing

## Installation

- Requires .NET 9 SDK, or .NET 9 ASP.NET Core and .NET runtimes installed on the host machine
- Unit tests and Docker compose require a Docker installation on the host machine

## Running

The solution can be run on the local machine natively; it makes use of .NET user secrets, so a user secrets file should be defined for the project in your IDE, which supplies a value for `ConnnectionStrings:DefaultConnection` that would connect to your local PostgresSQL instance. Alternatively a default connection string is specified in `appsettings.json`, but this will not necessarily work with the authentication details of your installation.

For ease of testing and deployment across multiple environments, a `docker-compose.yml` file is specified. Run this either through your IDE or by executing `docker-compose up -d` in the root directory. The composed cluster will run both the API and the postgres database required for the application to function. Through docker-compose, the application will be available on HTTP port `5096` at localhost.

At first execution either on the local machine or in Docker, the database will be seeded from the CSV that is committed to the repo, and any migrations required will be applied.

## Architecture

Some thought was put in to how to design the solution, in terms of balancing a demonstration of production-grade knowledge around C# application development with the relatively simple requirements of the app's specification. To strike a middle ground, I aimed to create a solution which could easily be scaled into a larger, modular monolithic project. I restrained a little to avoid the application architecture becoming more obtuse than the functionality it was providing really called for.

The main project is split into a few namespaces:

- Api: this contains the endpoints and any code specific to ASP.NET
- Domain: this contains the implementation for the data retrieval service, the data context and model, and any other EF Core related code
- Core: this contains the reusable models and interfaces used for both of the other namespaces

If the application was to require scaling to support multiple host environments, it would be straightforward to move the above namespaces into their own projects and provide some DI extensions to register their features in any dependencies. We would then have a reusable API implementation that could be used by multiple web applications. 

For example, as a public API, again as a subset of another private API, and perhaps then an API hosted by a larger Blazor application. The EFCore implementation of the domain could be swapped out to use Dapper. Additionally, say we needed a Discord bot that accessed the same data, we could create a project for that and use the Domain+Core layers. A separate application could even inherit only the Core layer and re-implement the `IMovieService` as a client to the API application to communicate with the database indirectly.

This level of abstraction was left out of the solution to make it easier to understand, given the specified requirements do not really call for modularization at this stage.

The API is versioned to allow for easy migration to a new API design in the future if required.

### Error handling

An application-wide exception handler is defined on the server to map exceptions to `Problem` types, with some additional specific mapping of status codes for bad requests due to missing, malformed, or invalid body. Besides the issues with
request validation, no exceptions are really expected to be encountered during the app's lifetime with the functionality that is currently in place. As such, no other exception handling is used throughout the app as any exception would truly be *exceptional*, and should be managed by the primary exception middleware.

## Testing

For ease of trying out and interacting with the API, an OpenAPI definition is generated and a Scalar UI for interacting with it is served at `/scalar/v1`. Under docker, the server will be running over http at port `5096`. For running directly on the host, HTTPS can be used, with a default port of `7192`. The launch configuration will automatically load the Scalar UI when the app is running.

A unit test project using xUnit is included which tests that the project meets all user requirements. These tests are run against PostgresSQL rather than using EF Core's in memory provider. The connection string is hardcoded to use the instance provided by the Docker compose cluster, so it must be running before unit tests are executed.

## Problems Encountered

The supplied CSV did not include any actor data. To provide this, the Bogus library was used to create a pool of 100 random names and assign 2–8 of these at random to each movie during seeding.

An additional problem was encountered with the CSV, where one of the entries had an Overview field which spilled onto multiple lines but was not quoted. This prevented CsvParser from being able to parse the file correctly. Some alternative parsing approaches (and providers) were tried, but it turned out these were silently failing and simply skipping the record. 

To solve this, I implemented some custom logic to fix the file before parsing; this resulted in all movies being successfully imported. The logic for this is inside the `SeedData` class.

## Future scope

Dependent on how user requirements develop, some possible avenues for further development could be:

- More endpoints, such as to find a single movie by ID, to create a movie, or update a movie
- A logging provider such as Serilog could be used, especially once the application grew to a point where we would need to log expected error cases, or submit logs to a remote service
- Performance could be improved by adding a cache such as FusionCache, which would allow a multi-node deployment with caching and backplane invalidation via Redis
- The project could be split into multiple modules as mentioned before, to aid reusability if several applications were needed to rely on the functionality provided
- The Actors and Genres fields could be split off into separate tables, this would allow us to provide endpoints to retrieve a list of valid filters for the search to display in a UI for selection
- Integration tests could be added using the ASP.NET Core testing package to verify the validation logic of the endpoint(s)
- A simple standalone UI could be developed to interact with the API. My tech stack of choice for this would have been React with TypeScript, Tanstack Router, Tanstack Query, and shadcn/ui (Tailwind)

## Notes

- The Actors and Genres arrays make use of Postgres array type mapping, a feature seldom found in other databases. This prevents the need for string splitting or introducing new tables for the data. It comes with some drawbacks, such as being unable to use the EF core in memory provider for testing or switch to another provider without remodeling.
