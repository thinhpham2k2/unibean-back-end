using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Parameters;

public class PagingModel
{
    /// <summary>
    /// Sorting criteria for the results.
    /// </summary>
    [DefaultValue("Id,desc")]
    public string Sort { get; set; }

    /// <summary>
    /// Search query.
    /// </summary>
    [DefaultValue("")]
    public string Search { get; set; }

    /// <summary>
    /// Current page in the paginated results.
    /// </summary>
    [DefaultValue("1")]
    [Range(1, int.MaxValue, ErrorMessage = "The page must be positive")]
    public int Page { get; set; }

    /// <summary>
    /// Number of results per page.
    /// </summary>
    [DefaultValue("10")]
    [Range(1, int.MaxValue, ErrorMessage = "The limit must be positive")]
    public int Limit { get; set; }
}
