using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KMS.Common.Helper
{
    public class CompanyResponsive
    {
        /// <summary>
        /// Tên công ty
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string? enterprise_Gdt_Code { get; set; }

        /// <summary>
        /// Đại diện pháp luật
        /// </summary>
        public string? legal_First_Name { get; set; }

        /// <summary>
        /// Địa chỉ công ty
        /// </summary>
        public string? ho_Address { get; set; }
    }
    public class CompanyViewModel
    {
        public string? TaxCode { set; get; }
        public string? FullNameVi { set; get; }
        public string? FullNameEn { set; get; }
        public string? Address { set; get; }
        public string? LegalRepresentative { set; get; }
    }
    public static class CompanyHelper
    {
        public static async Task<CompanyViewModel> GetCompanyAsync(this string taxNumber)
        {
            CompanyViewModel company = new CompanyViewModel();
            try
            {
                var jsonString = await ApiHelper.GetApiAsync("https://crm.phudev.com/Company/GetTaxCode?taxCode=" + taxNumber);
                var data = await jsonString.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<CompanyResponsive>>(data ?? "") ?? new List<CompanyResponsive>();

                var companyResponsive = result.FirstOrDefault(x => (x.enterprise_Gdt_Code ?? "").Trim().ToUpper().Equals((taxNumber ?? "").Trim().ToUpper())) ?? new CompanyResponsive();
                company.FullNameVi = companyResponsive.name;
                company.TaxCode = companyResponsive.enterprise_Gdt_Code;
                company.Address = companyResponsive.ho_Address;
                company.LegalRepresentative = companyResponsive.legal_First_Name;
            }
            catch
            {
                return company;
            }
            return company;
        }
    }
}
