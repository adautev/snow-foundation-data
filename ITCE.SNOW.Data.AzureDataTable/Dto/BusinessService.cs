using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ITCE.SNOW.Data.AzureDataTable.Dto
{
    public sealed class BusinessService : TableEntity
    {
        public string Name { get; set; }
        public string AssignmentGroup { get; set; }
        public string BusinessCriticality { get; set; }
        public string Manager { get; set; }
        public string Country { get; set; }
        public string Comments { get; set; }

        public BusinessService()
        {
            RowKey = Guid.NewGuid().ToString();
        }

        public BusinessService(Domain.BusinessService service, string partitionKey)
        {
            this.Name = service.Name;
            this.AssignmentGroup = service.AssignmentGroup;
            this.BusinessCriticality = service.BusinessCriticality;
            this.Manager = service.Manager?.Name;
            this.Country = service.Country;
            this.Comments = service.Comments;
            PartitionKey = partitionKey;
            RowKey = Guid.NewGuid().ToString();
        }
    }
}
