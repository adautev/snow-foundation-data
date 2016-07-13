using System;
using System.Collections.Generic;
using ITCE.SNOW.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace ITCE.SNOW.Data.AzureDataTable.Dto
{
    public class SupportGroupMember : TableEntity
    {
        public virtual string SupportGroupName { get; set; }
        public virtual string Country { get; set; }
        public virtual string Name { get; set; }
        
        public SupportGroupMember()
        {
            PartitionKey = Guid.NewGuid().ToString();
        }

        public SupportGroupMember(string groupName, string country, string name, string partitionKey)
        {
            Name = name;
            SupportGroupName = groupName;
            Country = country;
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = partitionKey;
        }
    }
}

