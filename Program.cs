using Grpc.Net.Client;
using MagicOnion.Client;
using Hello.Shared;
//using Microsoft.AspNetCore.Mvc;

var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = MagicOnionClient.Create<IHelloService>(channel);

var result = await client.SayAsync("World");
Console.WriteLine(result);
