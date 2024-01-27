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

            creation.OrderDetails = creation.OrderDetails.Select(
                o =>
            {
                o.OrderId = creation.Id;
                o.Price = db.Products.Where(s
                    => s.Id.Equals(o.ProductId) && (bool)s.Status).FirstOrDefault().Price;
                o.Amount = o.Price * o.Quantity;
                return o;
            }).ToList();

            creation.OrderStates = new List<OrderState>() { new OrderState
            {
                Id = Ulid.NewUlid().ToString(),
                OrderId = creation.Id,
                StateId = db.States.Where(s => (bool)s.Status).FirstOrDefault().Id,
                DateCreated = DateTime.Now,
                Description = creation.Description,
                States = true,
                Status = true
            }};

            // Update product quantity
            foreach (var detail in creation.OrderDetails)
            {
                Product product = db.Products.Where(s
                    => s.Id.Equals(detail.ProductId) && (bool)s.Status).FirstOrDefault();
                if (product != null && product.Quantity >= detail.Quantity)
                {
                    product.Quantity -= detail.Quantity;
                    db.Products.Update(product);
                }
                else
                {
                    throw new Exception("Invalid product or quantity");
                }
            }

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
        (List<string> stationIds, List<string> studentIds, List<string> stateIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
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
                    .Where(s => (bool)s.Status).Max(d => d.State.Id)))
                && (state == null || state.Equals(o.State))
                && (bool)o.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(o => o.Student)
               .Include(o => o.Station)
               .Include(o => o.OrderDetails.Where(d => (bool)d.Status))
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Images.Where(i => (bool)i.Status))
               .Include(o => o.OrderStates.Where(s => (bool)s.Status))
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
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(o => o.Student)
            .Include(o => o.Station)
            .Include(o => o.OrderDetails.Where(d => (bool)d.Status))
                .ThenInclude(o => o.Product)
                    .ThenInclude(p => p.Images.Where(i => (bool)i.Status))
            .Include(o => o.OrderStates.Where(s => (bool)s.Status))
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
