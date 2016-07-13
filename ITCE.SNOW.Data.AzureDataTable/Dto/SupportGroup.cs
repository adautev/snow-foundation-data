using System;
using System.Collections.Generic;
using ITCE.SNOW.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace ITCE.SNOW.Data.AzureDataTable.Dto
{
    public sealed class SupportGroup : TableEntity
    {
        public string Country { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public string Email { get; set; }

        public SupportGroup()
        {
            PartitionKey = Guid.NewGuid().ToString();
        }

        public SupportGroup(Domain.SupportGroup group)
        {
            MapFromDomain(@group);
            PartitionKey = Guid.NewGuid().ToString();
        }

        private void MapFromDomain(Domain.SupportGroup @group)
        {
            Country = @group.Country;
            Name = @group.Name;
            Manager = @group.Manager?.Name;
            Email = @group.Email;
            RowKey = Guid.NewGuid().ToString();
        }

        public SupportGroup(Domain.SupportGroup group, string partitionKey)
        {
           MapFromDomain(group);
           PartitionKey = partitionKey;
        }
    }
}
