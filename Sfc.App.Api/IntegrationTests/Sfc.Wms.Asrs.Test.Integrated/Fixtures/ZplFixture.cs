using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures
{
    public class ZplFixture
    {
        protected string path = "../../Reports/ZplDataTemplate.txt";
        protected ZplDto zplDto;


        protected void AValidZplRecord()
        {
            zplDto = new ZplDto
            {
                CartonTotalQtyDesc = "CASE"
            };
        }


        protected void ZplDataReplace()
        {
            using (StreamReader sr = new System.IO.StreamReader(path))
            {
                string fileLocMove = "";
                string newpath = Path.GetDirectoryName(path);
                fileLocMove = newpath + "\\" + "new.prn";
                string text = File.ReadAllText(path);
                text = text.Replace(ZplFieldNames.CartonTotalQtyDesc, zplDto.CartonTotalQtyDesc);
                text = text.Replace(ZplFieldNames.PalletId,zplDto.PalletId);
                text = text.Replace(ZplFieldNames.Flags1,zplDto.Flags1);
                text = text.Replace(ZplFieldNames.Flags2,zplDto.Flags2);
                text = text.Replace(ZplFieldNames.CartonTotalQty,zplDto.CartonTotalQty);
                text = text.Replace(ZplFieldNames.Level,zplDto.Level);
                text = text.Replace(ZplFieldNames.Bay,zplDto.Bay);
                text = text.Replace(ZplFieldNames.Aisle,zplDto.Aisle);
                text = text.Replace(ZplFieldNames.Area,zplDto.Area);
                text = text.Replace(ZplFieldNames.ReverseCode1,zplDto.ReverseCode1);
                text = text.Replace(ZplFieldNames.ReverseCode2,zplDto.ReverseCode2);
                text = text.Replace(ZplFieldNames.ShipTo,zplDto.ShipTo);
                text = text.Replace(ZplFieldNames.ShipToName,zplDto.ShipToName);
                text = text.Replace(ZplFieldNames.Line,zplDto.Line);
                text = text.Replace(ZplFieldNames.PktSeqNbr,zplDto.PktSeqNbr);
                text = text.Replace(ZplFieldNames.ActlDockActlDoor,zplDto.ActlDockActlDoor);
                text = text.Replace(ZplFieldNames.TempZone,zplDto.TempZone);
                text = text.Replace(ZplFieldNames.ShpmtNbr,zplDto.ShpmtNbr);
                text = text.Replace(ZplFieldNames.CartonNbrBc,zplDto.CartonNbrBc);
                text = text.Replace(ZplFieldNames.CaseCount,zplDto.CaseCount);
                text = text.Replace(ZplFieldNames.ShipDateTime,zplDto.ShipDateTime);
                text = text.Replace(ZplFieldNames.CustDept,zplDto.CustDept);
                text = text.Replace(ZplFieldNames.ShipDateTime,zplDto.ShipDateTime);
                text = text.Replace(ZplFieldNames.CustDept,zplDto.CustDept);
                text = text.Replace(ZplFieldNames.CustDept, zplDto.CustDept);
                text = text.Replace(ZplFieldNames.Style, zplDto.Style);
                text = text.Replace(ZplFieldNames.WaveNbr,zplDto.WaveNbr);
                text = text.Replace(ZplFieldNames.NestVolDfltUom, zplDto.NestVolDfltUom);
                text = text.Replace(ZplFieldNames.VendorItemNbr,zplDto.VendorItemNbr);
                text = text.Replace(ZplFieldNames.SkuDesc, zplDto.SkuDesc);
                text = text.Replace(ZplFieldNames.XofY, zplDto.XofY);
                text = text.Replace(ZplFieldNames.Quant, zplDto.Quant);
                File.WriteAllText(fileLocMove, text);
            }
        }
    }
}