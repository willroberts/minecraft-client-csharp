# minecraft-client-csharp

A client for the Minecraft RCON API., written in C# 9.0.

## Library Usage

```csharp
MinecraftClient client = new MinecraftClient("127.0.0.1", 25575);
client.Authenticate("password");
Message seedResp = client.SendCommand("seed");
Console.WriteLine(Encoding.ASCII.GetString(seedResp.Body)); // Seed: [1871644822592853811]
```

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