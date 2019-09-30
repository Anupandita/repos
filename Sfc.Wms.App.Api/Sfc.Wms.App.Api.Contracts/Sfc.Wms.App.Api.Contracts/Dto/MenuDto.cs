using System.Collections.Generic;

namespace Wms.App.Contracts.Dto
{
    public class MenuDto
    {
        public string DisplayName { get; set; }

        public List<MenuDto> Children { get; set; }

        public object RouteTo { get; set; }

        public int? MenuId { get; set; }
    }
}