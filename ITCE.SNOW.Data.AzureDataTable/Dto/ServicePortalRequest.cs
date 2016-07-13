using System;
using ITCE.SNOW.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace ITCE.SNOW.Data.AzureDataTable.Dto
{
    public sealed class ServicePortalRequest : TableEntity
    {
        public  string Name { get; set; }
        public  string BusinessService { get; set; }
        public  string SubCategory { get; set; }
        public  string Category { get; set; }
        public  string Approver { get; set; }
        public  string AssignmentGroup { get; set; }
        public  string Description { get; set; }
        public  string Country { get; set; }
        public ServicePortalRequest() { }

        public ServicePortalRequest(Domain.ServicePortalRequest request, string partitionKey)
        {
            PartitionKey = partitionKey;
            RowKey = Guid.NewGuid().ToString();
            this.Name = request.Name;
            this.BusinessService = request.BusinessService;
            this.SubCategory = request.SubCategory;
            this.Category = request.Category;
            this.Approver = request.Approver?.Name;
            this.AssignmentGroup = request.AssignmentGroup;
            this.Description = request.Description;
            this.Country = request.Country;
        }
    }
}
