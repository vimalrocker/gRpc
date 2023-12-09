﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using gRPC;
using Grpc.Core;
using Grpc.Net.Client;

namespace gRPCClient
{
    class Program
    {
        private const string Url = "https://localhost:5001/";
        
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress(Url);
            var client = new Greeter.GreeterClient(channel);

            var helloRequest = new HelloRequest
            {
                Name = "gRPC Client Demo"
            };
            var response = client.SayHello(helloRequest);

            Console.WriteLine(response.Message);

            Console.WriteLine("------------");
            
            Console.WriteLine("Calling the Customer Service in  Unary manner");
            
             var customerReply = await CallCustomerService();  
             Console.WriteLine(customerReply);
            
            Console.WriteLine("------------");

            await GetCustomerStream();
        }


        //Unary call 
        static async Task<CustomerResponse> CallCustomerService()
        {
            var channel = GrpcChannel.ForAddress(Url);
            var client = new Customer.CustomerClient(channel); 
            var request = new CustomerRequest { Id = 1 };
            var response = await client.GetCustomerAsync(request);
             
            return response;
        }


        static async Task GetCustomerStream()
        {
            Console.WriteLine("Calling the Customer Service -- Server Streaming");
            
            var channel = GrpcChannel.ForAddress(Url);
            var client = new Customer.CustomerClient(channel);
            var request = new CustomerRequest { Id = 1 };

            using var call = client.GetCustomerStream(request);

            var responseStream = call.ResponseStream;
            while (await responseStream.MoveNext())
            {
                var customer = responseStream.Current.Customerdetails.First();
                if (customer != null)
                {
                    Console.WriteLine($"Id: {customer.Id}, Name: {customer.Name}, Account Status: {customer.Accountstatus}");
                }
                else
                {
                    Console.WriteLine("Error: Customer response is empty.");
                }
            }
            
        }
    }
}