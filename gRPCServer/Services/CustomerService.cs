// Copyright 2023-2023 NXGN Management, LLC. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace gRPC.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }
        
        public override Task<CustomerResponse> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            CustomerModel model;
            if (request.Id == 1)
            {
                model = new CustomerModel
                {
                    Id = request.Id,
                    Name = "John Doe",
                    Active = true,
                    Dob = Timestamp.FromDateTime(new DateTime(1990,4,3,0,0,0,DateTimeKind.Utc)),
                    Age = Duration.FromTimeSpan(new TimeSpan(365, 20, 15, 12)),
                    Customercodeuid  = Guid.NewGuid().ToString(),
                    Accountstatus = AccountStatus.Active,
                     
                
                };
            }
            else
            {
                model = new CustomerModel
                {
                    Id = request.Id,
                    Name = "John Doe",
                    Active = true,
                    Dob = Timestamp.FromDateTime(new DateTime(1990,4,3,0,0,0,DateTimeKind.Utc)),
                    Age = Duration.FromTimeSpan(new TimeSpan(365, 20, 15, 12)),
                    Customercodeuid  = Guid.NewGuid().ToString(),
                    Accountstatus = AccountStatus.Suspended
                
                };
            }

            var response = new CustomerResponse
            {
                Customerdetails = { model }
            };
            return Task.FromResult(response); 
        }
        
        //Server streaming
        public  override async Task GetCustomerStream(CustomerRequest request, IServerStreamWriter<CustomerResponse> responseStream, ServerCallContext context)
        {
            // Assume fetching customer details from a database
            var customerDetails = new List<CustomerModel>
            {
                new CustomerModel
                {
                    Id = 1, Name = "John Doe",
                    Active = true, 
                    Dob = Timestamp.FromDateTime(DateTime.UtcNow), 
                    Age = Duration.FromTimeSpan(TimeSpan.FromDays(3650)), 
                    Customercodeuid = Guid.NewGuid().ToString(),
                    Accountstatus = AccountStatus.Active, 
                    Address =  "123 Main St"
                },
                new CustomerModel
                {
                Id = 2, Name = "John Doe2",
                Active = true, 
                Dob = Timestamp.FromDateTime(DateTime.UtcNow), 
                Age = Duration.FromTimeSpan(TimeSpan.FromDays(3650)), 
                Customercodeuid = Guid.NewGuid().ToString(),
                Accountstatus = AccountStatus.Active, 
                Address =  "123 Main St2"
            }
            };

            foreach (var customer in customerDetails)
            {
                await responseStream.WriteAsync(new CustomerResponse { Customerdetails = { customer } });
                Thread.Sleep(5000);
            }
        }
        
        //Client streaming RPC
        public override async  Task<CustomerResponse> SendCustomerStream(IAsyncStreamReader<CustomerRequest> requestsStream, ServerCallContext context)
        { 
            await foreach (var request in requestsStream.ReadAllAsync())
            {
               
                Console.WriteLine(request.Id);

            }
            
            return new CustomerResponse
            {
                Customerdetails = { GetCustomerDetails() }
            };

        }

        private static CustomerModel  GetCustomerDetails()
        {
            var customerDetails = new CustomerModel
            {
                Id = 1, Name = "John Doe",
                Active = true,
                Dob = Timestamp.FromDateTime(DateTime.UtcNow),
                Age = Duration.FromTimeSpan(TimeSpan.FromDays(3650)),
                Customercodeuid = Guid.NewGuid().ToString(),
                Accountstatus = AccountStatus.Active,
                Address = "123 Main St"
            };
            return  customerDetails;
        }

        /*
        The gRPC method types are:
https://learn.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-7.0
            Unary
            Server streaming
            Client streaming
            Bi-directional streaming*/

    }
}