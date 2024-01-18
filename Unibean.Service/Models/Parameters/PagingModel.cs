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
    [Required(ErrorMessage = "sort property is required")]
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
    [Required(ErrorMessage = "page is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The page must be positive")]
    public int Page { get; set; }

    /// <summary>
    /// Number of results per page.
    /// </summary>
    [DefaultValue("10")]
    [FromQuery(Name = "limit")]
    [Required(ErrorMessage = "limit is required")]
    [Range(1, int.MaxValue, ErrorMessage = "The limit must be positive")]
    public int Limit { get; set; }
}
