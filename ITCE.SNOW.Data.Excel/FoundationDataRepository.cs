using System.Collections.Generic;
using System.IO;
using System.Linq;
using Excel;
using ITCE.SNOW.Data.Excel.Comparers;
using ITCE.SNOW.Data.Excel.Extensions;
using ITCE.SNOW.Domain;
using Dto = ITCE.SNOW.Dtos.V1;

namespace ITCE.SNOW.Data.Excel
{
    public class FoundationDataRepository
    {
        public const string SupportGroupsSheetName = "Support Groups";
        public IEnumerable<SupportGroup> GetSupportGroups(Stream source)
        {
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(source))
            {
                excelReader.IsFirstRowAsColumnNames = true;
                var result = excelReader.AsDataSet();
                var dtoGroups =  result.Tables["Support Groups"].ToList<Dto.SupportGroup>();
                var groups =
                    dtoGroups.Distinct(new SupportGroupComparer()).Select( sg=> new Domain.SupportGroup
                    {
                        Country = sg.Country,
                        Name = sg.Name,
                        Reference = sg.Reference,
                        Email = sg.Email,
                        Manager = sg.Manager,
                        Members = dtoGroups.Where(
                            g=>g.Name == sg.Name && g.Country == sg.Country
                            )
                            .Select(g=>new User(g.Member)).ToList()
                    });
                return groups;
            }
            
        }
        public IEnumerable<BusinessService> GetBusinessServices(Stream source)
        {
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(source))
            {
                excelReader.IsFirstRowAsColumnNames = true;
                var result = excelReader.AsDataSet();
                var dtoServices = result.Tables["Business Services"].ToList<Dto.BusinessService>();
                var commonServices = dtoServices.Where(s => s.Country != "GR" && s.Country != "US").ToList();
                dtoServices.RemoveAll(s=>commonServices.Any(cs=>cs==s));
                var services =
                    dtoServices.Distinct(new BusinessServiceComparer()).Select(bs => new BusinessService
                    {
                        Country = bs.Country,
                        Comments = bs.Comments,
                        Name = bs.Name,
                        Reference = bs.Reference,
                        Manager = bs.Country == "US" ? bs.ManagerUs : bs.ManagerGr,
                        AssignmentGroup = bs.Country == "US" ? bs.AssignmentGroupUs : bs.AssignmentGroupGr,
                        BusinessCriticality = bs.Country == "US" ? bs.BusinessCriticalityUs : bs.BusinessCriticalityGr,
                    }).ToList();
                //Add common services for US
                services.AddRange(commonServices.Select(bs => new BusinessService
                {
                    Country = "US",
                    Name = bs.Name,
                    Comments = bs.Comments,
                    Reference = bs.Reference,
                    Manager = bs.ManagerUs,
                    AssignmentGroup = bs.AssignmentGroupUs,
                    BusinessCriticality = bs.BusinessCriticalityUs
                }));
                //Add common services for GR
                services.AddRange(commonServices.Select(bs => new BusinessService
                {
                    Country = "GR",
                    Name = bs.Name,
                    Comments = bs.Comments,
                    Reference = bs.Reference,
                    Manager = bs.ManagerGr,
                    AssignmentGroup = bs.AssignmentGroupGr,
                    BusinessCriticality = bs.BusinessCriticalityGr
                }));
                return services;
            }

        }
        public IEnumerable<ServicePortalRequest> GetServicePortalRequests(Stream source)
        {
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(source))
            {
                excelReader.IsFirstRowAsColumnNames = true;
                var result = excelReader.AsDataSet();
                var dtoRequests = result.Tables["ServicePortalRequests"].ToList<Dto.ServicePortalRequest>();
                return dtoRequests.Select(r => new ServicePortalRequest
                {
                    BusinessService = r.BusinessService,
                    Category = r.Category,
                    SubCategory = r.SubCategory,
                    Country = r.Country,
                    Approver = r.Approver,
                    AssignmentGroup = r.AssignmentGroup,
                    Description = r.Description,
                    Name = r.Name,
                    Reference = r.Reference
                });
            }

        }
    }
}
