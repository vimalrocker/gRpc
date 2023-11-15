// Copyright 2023-2023 NXGN Management, LLC. All Rights Reserved.

using System;
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
        
        /*
        The gRPC method types are:
https://learn.microsoft.com/en-us/aspnet/core/grpc/client?view=aspnetcore-7.0
            Unary
            Server streaming
            Client streaming
            Bi-directional streaming*/

    }
}