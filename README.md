# minecraft-client-csharp

A client for the Minecraft RCON API., written in C# 9.0.

## Current Usage

The values for `Host`, `Port`, and `Password` are constants in [`Shell.cs`](src/MinecraftClient/Shell.cs). 
The default values will work when using the above Docker image.

```bash
$ dotnet run
establishing connection
authenticating
getting world seed
Seed: [1871644822592853811]
disconnecting
```

## Library Usage

TBD

## Shell Utility

TBD

## Limitations

Response bodies over 4KB will be truncated.

## Starting a server for testing

```
$ docker pull itzg/minecraft-server
$ docker run --name=minecraft-server -p 25575:25575 -d -e EULA=TRUE itzg/minecraft-server
```

## Running Tests

After starting the test server in Docker:

```
$ dotnet test
```

## Reference

- https://wiki.vg/Rcon