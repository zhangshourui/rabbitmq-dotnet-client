## RabbitMQ .NET Client

[![AppVeyor Build status](https://ci.appveyor.com/api/projects/status/33srpo7owl1h3y4e?svg=true)](https://ci.appveyor.com/project/rabbitmq/rabbitmq-dotnet-client)
![GitHub Actions (Linux)](https://github.com/rabbitmq/rabbitmq-dotnet-client/workflows/Build%20(Linux)/badge.svg)

This repository contains source code of the [RabbitMQ .NET client](https://www.rabbitmq.com/dotnet.html).
The client is maintained by the [RabbitMQ team at VMware](https://github.com/rabbitmq/).


## Dependency (NuGet Artifact)

The client is [distributed via NuGet](https://www.nuget.org/packages/RabbitMQ.Client/).


## Tutorials and Documentation

 * [Tutorials](https://www.rabbitmq.com/getstarted.html)
 * [Documentation guide](https://www.rabbitmq.com/dotnet.html)
 * [API Documentation](https://rabbitmq.github.io/rabbitmq-dotnet-client/index.html)


## Supported Platforms and .NET Releases

### 6.x

`6.x` versions of the library require .NET 4.6.1 or a .NET Core version implementing .NET Standard 2.0.
They also introduce potentially breaking public API changes covered in the [changelog](CHANGELOG.md).

### 5.x and 4.x

`4.x` and `5.x` versions of the library require .NET 4.5.1 or later or .NET Core.
For .NET Core users, 2.0 is the minimum supported version for `5.x` series.


## Change Log

See [CHANGELOG.md](CHANGELOG.md).


## Building from Source

Please see [How to Run Tests](RUNNING_TESTS.md) for the build and test process overview.


## Contributing

See [Contributing](CONTRIBUTING.md) and [How to Run Tests](RUNNING_TESTS.md).


## License

This package, the RabbitMQ .NET client library, is double-licensed under
the Mozilla Public License 2.0 ("MPL") and the Apache License version 2 ("ASL").

This means that the user can consider the library to be licensed under **any of the licenses from the list** above.
For example, you may choose the Apache Public License 2.0 and include this client into a commercial product.
