using Grpc.Net.Client;
using MagicOnion.Client;
using Hello.Shared;
using Grpc.Core;
//using Microsoft.AspNetCore.Mvc;

namespace GrpcClient
{
  class Program
  {
    private const string address = "http://localhost:5000";

    private static string _token;

    private static GrpcChannel _channel;

    static async Task Main(string[] args)
    {
      
      CreateChannel();
      CreateAccount();
    }

    private static void CreateAuthenticatedChannel()
    {
      // 認証情報をセット
      var credentials = CallCredentials.FromInterceptor((context, metadata) => 
      {
        if (!string.IsNullOrEmpty(_token))
        {
          metadata.Add("Authorization", $"Bearer {_token}");
        }
        return Task.CompletedTask;
      });

      // 認証情報を使用して接続
      _channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
      {
        Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
        //Credentials = ChannelCredentials.Insecure
      });
    }

    private static void CreateChannel()
    {
      // 接続
      Console.WriteLine($"Connect server: {address}");
      var channel = GrpcChannel.ForAddress(address);
    }

    private static async void CreateAccount()
    {
      // 接続
      var client = MagicOnionClient.Create<IAccountService>(_channel);

      var result = await client.CreateAccountAsync("maru1");

      Console.WriteLine(result);
      Console.WriteLine(result.ToString());

      _token = result.ToString();
    }
  }
}

/*
var channel = GrpcChannel.ForAddress("http://localhost:5000");

var client = MagicOnionClient.Create<IHelloService>(channel);

var result = await client.SayAsync("World");
Console.WriteLine(result);
*/
