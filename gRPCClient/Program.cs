using System;
using System.Threading.Tasks;
using gRPC;
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
            
            Console.WriteLine("Calling the Customer Service");
            
             var customerReply = await CallCustomerService();  
             Console.WriteLine(customerReply);
            
            Console.WriteLine("------------");
        }


        static async Task<CustomerResponse> CallCustomerService()
        {
            var channel = GrpcChannel.ForAddress(Url);
            var client = new Customer.CustomerClient(channel); 
            var request = new CustomerRequest { Id = 1 };
            var response = await client.GetCustomerAsync(request);
             
            return response;
            
           
        }
    }
}