﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Repository.Repositories;

public class ActivityRepository : IActivityRepository
{
    public Activity Add(Activity creation)
    {
        try
        {
            using var db = new UnibeanDBContext();
            creation = db.Activities.Add(creation).Entity;
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
            var activity = db.Activities.FirstOrDefault(b => b.Id.Equals(id));
            activity.Status = false;
            db.Activities.Update(activity);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Activity> GetAll
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, 
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Activity> pagedResult = new();
        try
        {
            using var db = new UnibeanDBContext();
            var query = db.Activities
                .Where(t => (EF.Functions.Like((string)(object)t.Type, "%" + search + "%")
                || EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherItemId))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status))
                    .ThenInclude(a => a.Wallet)
                        .ThenInclude(w => w.Type)
               .Include(s => s.Store)
               .Include(s => s.Student)
               .Include(s => s.VoucherItem)
                    .ThenInclude(v => v.Voucher)
               .ToList();

            pagedResult = new PagedResultModel<Activity>
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

    public Activity GetById(string id)
    {
        Activity activity = new();
        try
        {
            using var db = new UnibeanDBContext();
            activity = db.Activities
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status))
                .ThenInclude(a => a.Wallet)
                    .ThenInclude(w => w.Type)
            .Include(s => s.Student)
            .Include(s => s.Store)
                .ThenInclude(s => s.Brand)
            .Include(s => s.VoucherItem)
                .ThenInclude(v => v.Voucher)
            .Include(s => s.VoucherItem)
                .ThenInclude(v => v.Campaign)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return activity;
    }

    public List<Activity> GetList
        (List<string> storeIds, List<string> studentIds, List<string> voucherIds, string search)
    {
        List<Activity> result;
        try
        {
            using var db = new UnibeanDBContext();
            result = db.Activities
                .Where(t => (EF.Functions.Like((string)(object)t.Type, "%" + search + "%")
                || EF.Functions.Like(t.Store.StoreName, "%" + search + "%")
                || EF.Functions.Like(t.Store.Address, "%" + search + "%")
                || EF.Functions.Like(t.Student.FullName, "%" + search + "%")
                || EF.Functions.Like(t.VoucherItem.Voucher.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (storeIds.Count == 0 || storeIds.Contains(t.StoreId))
                && (studentIds.Count == 0 || studentIds.Contains(t.StudentId))
                && (voucherIds.Count == 0 || voucherIds.Contains(t.VoucherItemId))
                && (bool)t.Status && t.Type.Equals(Type.Use))
               .Include(s => s.ActivityTransactions.Where(a => (bool)a.Status && a.Amount > 0))
                    .ThenInclude(a => a.Wallet)
                        .ThenInclude(w => w.Type)
               .Include(s => s.Store)
               .Include(s => s.Student)
               .Include(s => s.VoucherItem)
                    .ThenInclude(v => v.Voucher).ToList();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

    public Activity Update(Activity update)
    {
        try
        {
            using var db = new UnibeanDBContext();
            update = db.Activities.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
