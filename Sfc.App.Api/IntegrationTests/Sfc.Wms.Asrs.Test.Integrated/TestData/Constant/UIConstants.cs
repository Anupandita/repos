using Castle.Core.Internal;
using System.Data;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant
{
    public class UIConstants
    {
        public const string LoginUrl = "http://dev.az.app.api.wms.shamrockfoods.com/user/login";
        public const string ItemAttributes = "item-attributes/";
        public const string Lpn = "lpn/";
        public const string Search = "search?";
        public const string LpnComments = "lpn-comments";
        public const string LpnDetails = "lpn-details";
        public const string LpnHistory = "lpn-history";
        public const string Find = "find";
        public const string SearchInputItemId = "attributeSearchInputDto.itemId=";
        public const string SearchInputItemDescription = "attributeSearchInputDto.itemDescription=";
        public const string SearchInputTempZone = "attributeSearchInputDto.tempZone=";
        public const string SearchInputVendorItemNumber = "attributeSearchInputDto.vendorItemNumber=";
        public const string SearchInputTotalRecords = "attributeSearchInputDto.totalRecords=";
        public const string SearchInputLastPageNumber = "attributeSearchInputDto.lastPageNumber=";
        public const string SearchInputPageNumber = "attributeSearchInputDto.pageNumber=";
        public const string SearchInputPageSize = "attributeSearchInputDto.pageSize=";
        public const string SearchInputSortOptions = "attributeSearchInputDto.sortOptions=";
        public const string DateTimeFormat = "mm-dd-yy hh24:mi:ss";
        public const string FormatDateTime = "mm/dd/yyyy hh24:mi:ss";
        public const string EnterDateTimeFormat = "MM/dd/yyyy";
        public const string VolumeDecimalFormat = "99990.0000";
        public const string DecimalFormat = "99990.00";
        public const string HeightFormat = "99990.0";
        public static string ItemNumber;
        public static string CartonNbr;
        public static string PoNumber;
        public static string LpnNumber;
        public static string DisplayLocation;
        public static string Aisle;
        public static string Zone;
        public static string Level;
        public static string Slot;
        public const string FromStatus = "0";
        public const string ToStatus = "0";
        public const string Whse = "008";
        public static string QvPoNbr;
        public const string LpnToStatus = "50";
        public const string LpnFromStatus = "45";
        public static string AdjacentLocation;
        public static string LpnNbrForLockUnlock;
        public static string LpnNumberForItems;
        public static string LpnNumberForHistory;
        public static string BearerToken;
        public static string ItemDescription;
        public static string VendorItemNumber;
        public static string TempZone;
        public static string VendorItemNumberCount;
        public static string TempZoneCount;
        public static DataTable UpdateTable(DataTable dt, bool isGrid)
        {
            var temp = new DataTable();
            int rnum = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                temp.Columns.Add(dc.ColumnName);
            }
            foreach (DataRow dr in dt.Rows)
            {
                temp.Rows.Add();
                DataRow drTemp = temp.Rows[rnum];
                rnum++;
                foreach (DataColumn dc in dt.Columns)
                {
                    DataColumn dcTemp = temp.Columns[dc.ColumnName];
                    var current = dr[dc].ToString();
                    if (current == " ")
                    {
                        current = "";
                    }
                    if (!isGrid && current.IsNullOrEmpty())
                    {
                        drTemp[dcTemp] = "---No Data---";
                    }
                    else
                    { drTemp[dcTemp] = current; }
                }
            }
            return temp;
        }
    }
}
