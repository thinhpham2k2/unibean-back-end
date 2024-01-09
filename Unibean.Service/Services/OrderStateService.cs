﻿using Microsoft.IdentityModel.Tokens;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.OrderStates;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class OrderStateService : IOrderStateService
{
    private readonly IStateRepository stateRepository;

    private readonly IOrderRepository orderRepository;

    private readonly IOrderStateRepository orderStateRepository;

    public OrderStateService(IOrderRepository orderRepository,
        IOrderStateRepository orderStateRepository,
        IStateRepository stateRepository)
    {
        this.orderRepository = orderRepository;
        this.orderStateRepository = orderStateRepository;
        this.stateRepository = stateRepository;
    }

    public string Add(string id, CreateOrderStateModel creation)
    {
        Order entity = orderRepository.GetById(id);
        if (entity != null)
        {
            List<string> stateIds = entity.OrderStates.Select(s => s.StateId).ToList();
            if (!stateIds.IsNullOrEmpty())
            {
                if (string.Compare(stateIds.Max(), creation.StateId) < 0)
                {
                    stateIds = stateRepository.GetAll("Id", true, "", 1, 100).Result.Select(s 
                        => s.Id).Where(s => string.Compare(s, stateIds.Max()) > 0 
                        && string.Compare(s, creation.StateId) <= 0).ToList();

                    stateIds.ForEach(s =>
                    {
                        orderStateRepository.Add(new OrderState
                        {
                            Id = Ulid.NewUlid().ToString(),
                            OrderId = id,
                            StateId = s,
                            DateCreated = DateTime.Now,
                            Description = creation.Description,
                            States = true,
                            Status = true,
                        });
                    });

                    return "Created successfully";
                }
                else
                {
                    throw new InvalidParameterException("Invalid state because the order has already gone through this state");
                }
            }
            else
            {
                stateIds = stateRepository.GetAll("Id", true, "", 1, 100).Result.Select(s
                        => s.Id).Where(s => string.Compare(s, creation.StateId) <= 0).ToList();
                stateIds.ForEach(s =>
                {
                    orderStateRepository.Add(new OrderState
                    {
                        Id = Ulid.NewUlid().ToString(),
                        OrderId = id,
                        StateId = s,
                        DateCreated = DateTime.Now,
                        Description = creation.Description,
                        States = true,
                        Status = true,
                    });
                });

                return "Created successfully";
            }
        }
        throw new InvalidParameterException("Invalid order");
    }
}
