using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class OrderStateRepository : IOrderStateRepository
{
    public OrderState Add(OrderState creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.OrderStates.Add(creation).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return creation;
    }

    public void Delete(string id)
    {
        try
        {
            using var db = new UnibeanDBContext();
            var orderState = db.OrderStates.FirstOrDefault(b => b.Id.Equals(id));
            orderState.Status = false;
            db.OrderStates.Update(orderState);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<OrderState> GetAll
        (List<string> orderIds, List<int> stateIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<OrderState> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.OrderStates
                .Where(t => (EF.Functions.Like((string)(object)t.State, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (orderIds.Count == 0 || orderIds.Contains(t.OrderId))
                && (stateIds.Count == 0 || stateIds.Contains((int)t.State))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Order)
               .Include(s => s.State)
               .ToList();

            pagedResult = new PagedResultModel<OrderState>
            {
                CurrentPage = page,
                PageSize = limit,
                PageCount = (int)Math.Ceiling((double)query.Count() / limit),
                Result = result,
                RowCount = result.Count,
                TotalCount = query.Count()
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return pagedResult;
    }

    public OrderState GetById(string id)
    {
        OrderState orderState = new();
        try
        {
            using var db = new UnibeanDBContext();
            orderState = db.OrderStates
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Order)
            .Include(s => s.State)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return orderState;
    }

    public OrderState Update(OrderState update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.OrderStates.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
