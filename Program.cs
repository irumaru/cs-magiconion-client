using Grpc.Net.Client;
using MagicOnion.Client;
using Hello.Shared;
using Grpc.Core;
//using Microsoft.AspNetCore.Mvc;

string address = "http://localhost:5001";

Console.WriteLine($"Connect server: {address}");
var channel = GrpcChannel.ForAddress(address);

var client = MagicOnionClient.Create<IAccountService>(channel);

var token = await client.CreateAccountAsync("maru1");

Console.WriteLine(token);


// 認証情報をセット
var credentials = CallCredentials.FromInterceptor((context, metadata) => 
{
  if (!string.IsNullOrEmpty(token))
  {
    metadata.Add("Authorization", $"Bearer {token}");
  }
  return Task.CompletedTask;
});

// 認証情報を使用して接続
var authChannel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
{
  //Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
  Credentials = ChannelCredentials.Create(ChannelCredentials.Insecure, credentials),
  UnsafeUseInsecureChannelCallCredentials = true
});

var userClient = MagicOnionClient.Create<IHelloService>(authChannel);

var hello = await userClient.SayAsync("World");

Console.WriteLine(hello);


/*
var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = MagicOnionClient.Create<IHelloService>(channel);

var result = await client.SayAsync("World");
Console.WriteLine(result);
*/
