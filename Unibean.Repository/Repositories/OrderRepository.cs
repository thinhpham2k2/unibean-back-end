using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class OrderRepository : IOrderRepository
{
    public Order Add(Order creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Orders.Add(creation).Entity;
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
            var order = db.Orders.FirstOrDefault(b => b.Id.Equals(id));
            order.Status = false;
            db.Orders.Update(order);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Order> GetAll
        (List<string> stationIds, List<string> studentIds, List<string> stateIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Order> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Orders
                .Where(o => (EF.Functions.Like(o.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(o.Station.StationName, "%" + search + "%")
                || EF.Functions.Like(o.Description, "%" + search + "%"))
                && (stationIds.Count == 0 || stationIds.Contains(o.StationId))
                && (studentIds.Count == 0 || studentIds.Contains(o.StudentId))
                && (stateIds.Count == 0 || stateIds.Contains(o.OrderStates
                    .Where(s => s.Status.Equals(true)).Max(d => d.State.Id)))
                && o.Status.Equals(true))
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(o => o.Student)
               .Include(o => o.Station)
               .Include(o => o.OrderDetails.Where(d => d.Status.Equals(true)))
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Images)
               .Include(o => o.OrderStates.Where(s => s.Status.Equals(true)))
                    .ThenInclude(o => o.State)
               .ToList();

            pagedResult = new PagedResultModel<Order>
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

    public Order GetById(string id)
    {
        Order order = new();
        try
        {
            using var db = new UnibeanDBContext();
            order = db.Orders
            .Where(s => s.Id.Equals(id) && s.Status.Equals(true))
            .Include(o => o.Student)
            .Include(o => o.Station)
            .Include(o => o.OrderDetails.Where(d => d.Status.Equals(true)))
                .ThenInclude(o => o.Product)
                    .ThenInclude(p => p.Images)
            .Include(o => o.OrderStates.Where(s => s.Status.Equals(true)))
                .ThenInclude(o => o.State)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return order;
    }

    public Order Update(Order update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Orders.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
