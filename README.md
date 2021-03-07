# minecraft-client-csharp

This is an RCON shell for Minecraft servers, written in C# 9.0.

## Starting the test server

```bash
$ docker pull itzg/minecraft-server
$ docker run --name=minecraft-server -p 25575:25575 -d -e EULA=TRUE itzg/minecraft-server
```

## Configuration

The values for `Host`, `Port`, and `Password` are currently constants in [`Program.cs`](MinecraftClient/Program.cs). The default values will work when using the above Docker image.

## Usage

```bash
$ dotnet run
establishing connection
authenticating
getting world seed
Seed: [1871644822592853811]
disconnecting
```