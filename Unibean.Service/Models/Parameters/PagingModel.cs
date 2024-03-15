using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Parameters;

public class PagingModel
{
    /// <summary>
    /// Sorting criteria for the results.
    /// </summary>
    [FromQuery(Name = "sort")]
    [DefaultValue("Id,desc")]
    [Required(ErrorMessage = "Thuộc tính sắp xếp là bắt buộc")]
    public string Sort { get; set; }

    /// <summary>
    /// Search query.
    /// </summary>
    [DefaultValue("")]
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    /// <summary>
    /// Current page in the paginated results.
    /// </summary>
    [DefaultValue("1")]
    [FromQuery(Name = "page")]
    [Required(ErrorMessage = "Số trang là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Số trang phải là số dương")]
    public int Page { get; set; }

    /// <summary>
    /// Number of results per page.
    /// </summary>
    [DefaultValue("10")]
    [FromQuery(Name = "limit")]
    [Required(ErrorMessage = "Số dòng là bắt buộc")]
    [Range(1, int.MaxValue, ErrorMessage = "Số dòng phải là số dương")]
    public int Limit { get; set; }
}
