using System.ComponentModel;
using Sfc.Core.OnPrem.Pagination;

namespace Sfc.Wms.App.Api.Contracts.Dto
{
    public class SystemCodeInputDto
    {
        public string RecType { get; set; }
        public string CodeType { get; set; }
        public string CodeId { get; set; }
        public SortOption SortOption { get; set; } = new SortOption { OrderBy = ListSortDirection.Descending, PropertyName = nameof(CodeId) };
    }
}
