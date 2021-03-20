# minecraft-client-csharp

A client for the Minecraft RCON protocol, written in C# 9.0.

## Library Usage

```csharp
// Create a new client and connect to the server.
MinecraftClient client = new MinecraftClient("127.0.0.1", 25575);

// Send some commands.
// Commands use the Try-Parse pattern for error handling instead of throwing Exceptions.
// Pass an uninitialized Message by reference to get a bool return value indicating success or failure.
// The Sockets library can still raise Exceptions you'd want to catch (e.g. connection failures).
if (!client.Authenticate("password")) { /* handle authentication error */ };
Message resp;
if (!client.SendCommand("seed", out resp)) { /* handle command error */ };
Console.WriteLine(resp.Body); // "Seed: [1871644822592853811]"

// Cleanly disconnect when finished.
client.Close();
```

## Shell Utility

If you are looking for a tool rather than a library, try the shell command:

```bash
$ cd src/MinecraftClient
$ dotnet run --host 127.0.0.1 --port 25575 --password minecraft
Starting RCON shell. Use 'exit', 'quit', or Ctrl-C to exit.
> list
There are 0 of a max of 20 players online:
> seed
Seed: [1871644822592853811]
```

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
$ cd src/MinecraftClient.Test
$ dotnet test
```

## Reference

- https://wiki.vg/Rcon