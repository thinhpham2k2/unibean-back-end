﻿using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using System.Linq.Dynamic.Core;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Repository.Repositories;

public class VoucherRepository : IVoucherRepository
{
    private readonly UnibeanDBContext unibeanDB;

    public VoucherRepository(UnibeanDBContext unibeanDB)
    {
        this.unibeanDB = unibeanDB;
    }

    public Voucher Add(Voucher creation)
    {
        try
        {
            var db = unibeanDB;
            creation = db.Vouchers.Add(creation).Entity;
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
            var db = unibeanDB;
            var voucher = db.Vouchers.FirstOrDefault(b => b.Id.Equals(id));
            voucher.Status = false;
            db.Vouchers.Update(voucher);
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public PagedResultModel<Voucher> GetAll
        (List<string> brandIds, List<string> typeIds, bool? state,
        string propertySort, bool isAsc, string search, int page, int limit)
    {
        PagedResultModel<Voucher> pagedResult = new();
        try
        {
            var db = unibeanDB;
            var query = db.Vouchers
                .Where(t => (EF.Functions.Like(t.VoucherName, "%" + search + "%")
                || EF.Functions.Like(t.Brand.BrandName, "%" + search + "%")
                || EF.Functions.Like(t.Type.TypeName, "%" + search + "%")
                || EF.Functions.Like(t.Condition, "%" + search + "%")
                || EF.Functions.Like(t.Description, "%" + search + "%"))
                && (brandIds.Count == 0 || brandIds.Contains(t.BrandId))
                && (typeIds.Count == 0 || typeIds.Contains(t.TypeId))
                && (state == null || state.Equals(t.State))
                && (bool)t.Status)
                .OrderBy(propertySort + (isAsc ? " ascending" : " descending"));

            var result = query
               .Skip((page - 1) * limit)
               .Take(limit)
               .Include(s => s.Brand)
               .Include(s => s.Type)
               .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
               .ToList();

            pagedResult = new PagedResultModel<Voucher>
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

    public Voucher GetById(string id)
    {
        Voucher voucher = new();
        try
        {
            var db = unibeanDB;
            voucher = db.Vouchers
            .Where(s => s.Id.Equals(id) && (bool)s.Status)
            .Include(s => s.Brand)
                .ThenInclude(b => b.Account)
            .Include(s => s.Type)
            .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
                .ThenInclude(v => v.CampaignDetail)
                    .ThenInclude(c => c.Campaign)
                        .ThenInclude(c => c.Type)
            .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
                .ThenInclude(v => v.CampaignDetail)
                    .ThenInclude(c => c.Campaign)
                        .ThenInclude(s => s.Brand)
            .Include(s => s.VoucherItems.Where(v => (bool)v.Status))
                .ThenInclude(v => v.CampaignDetail)
                    .ThenInclude(c => c.Campaign)
                        .ThenInclude(s => s.CampaignActivities.Where(a => (bool)a.Status).OrderBy(a => a.Id))
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return voucher;
    }

    public Voucher GetByIdAndCampaign(string id, string campaignId)
    {
        Voucher voucher = new();
        try
        {
            var db = unibeanDB;
            voucher = db.Vouchers
            .Where(s => s.Id.Equals(id) && (bool)s.Status
            && s.VoucherItems.Any(v => v.CampaignDetail.CampaignId.Equals(campaignId)))
            .Include(s => s.Brand)
            .Include(s => s.Type)
            .Include(s => s.VoucherItems.Where(
                v => (bool)v.Status && !(bool)v.IsBought && !(bool)v.IsUsed
                && v.CampaignDetail.CampaignId.Equals(campaignId)))
                .ThenInclude(v => v.CampaignDetail)
                    .ThenInclude(c => c.Campaign)
                        .ThenInclude(c => c.Type)
            .FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return voucher;
    }

    public Voucher Update(Voucher update)
    {
        try
        {
            var db = unibeanDB;
            update = db.Vouchers.Update(update).Entity;
            db.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return update;
    }
}
