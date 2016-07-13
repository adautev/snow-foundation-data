using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITCE.SNOW.Data.AzureDataTable.Dto;
using ITCE.SNOW.Domain;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using BusinessService = ITCE.SNOW.Data.AzureDataTable.Dto.BusinessService;
using ServicePortalRequest = ITCE.SNOW.Data.AzureDataTable.Dto.ServicePortalRequest;
using SupportGroup = ITCE.SNOW.Domain.SupportGroup;

namespace ITCE.SNOW.Data.AzureDataTable
{
    public class DataTableStorageRepository
    {
        private const string ServicePortalRequestTable = "servicePortalRequests";
        private const string BusinessServiceTable = "businessServices";
        private const string SupportGroupTable = "supportGroups";
        private const string SupportGroupMembersTable = "supportGroupMembers";
        private readonly CloudStorageAccount _storageAccount;

        public DataTableStorageRepository(DataTableSettings settings)
        {
            _storageAccount = CloudStorageAccount.Parse(settings.ConnectionString);
        }
        public async Task StoreSupportGroups(IEnumerable<SupportGroup> groups)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(SupportGroupTable);

            await table.CreateIfNotExistsAsync().ConfigureAwait(true);

            DeleteItems<Dto.SupportGroup>(table);
            var operation = new TableBatchOperation();
            var partitionKey = Guid.NewGuid().ToString();
            foreach (var supportGroup in groups)
            {
                operation.Insert(new Dto.SupportGroup(supportGroup, partitionKey));

            }
            await table.ExecuteBatchAsync(operation).ConfigureAwait(true);

        }
        public async Task StoreBusinessServices(IEnumerable<Domain.BusinessService> services)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(BusinessServiceTable);

            await table.CreateIfNotExistsAsync().ConfigureAwait(true);
            DeleteItems<Dto.BusinessService>(table);

            var operation = new TableBatchOperation();
            var partitionKey = Guid.NewGuid().ToString();
            var batchCount = 0;
            foreach (var service in services)
            {

                operation.Insert(new Dto.BusinessService(service, partitionKey));
                batchCount++;
                //Reset batch job
                if (batchCount < 20) continue;
                await table.ExecuteBatchAsync(operation).ConfigureAwait(true);
                batchCount = 0;
                operation = new TableBatchOperation();
            }
            await table.ExecuteBatchAsync(operation).ConfigureAwait(true);

        }
        public async Task StoreServicePortalRequests(IEnumerable<Domain.ServicePortalRequest> requests)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(ServicePortalRequestTable);

            await table.CreateIfNotExistsAsync().ConfigureAwait(true);
            DeleteItems<Dto.ServicePortalRequest>(table);

            var operation = new TableBatchOperation();
            var partitionKey = Guid.NewGuid().ToString();
            var batchCount = 0;
            foreach (var service in requests)
            {

                operation.Insert(new Dto.ServicePortalRequest(service, partitionKey));
                batchCount++;
                //Reset batch job
                if (batchCount < 20) continue;
                await table.ExecuteBatchAsync(operation).ConfigureAwait(true);
                batchCount = 0;
                operation = new TableBatchOperation();
            }
            await table.ExecuteBatchAsync(operation).ConfigureAwait(true);

        }
        private static void DeleteItems<T>(CloudTable table) where T : TableEntity, new()
        {
            var query = table.CreateQuery<T>();
            foreach (var item in query.Execute())
            {
                var deleteOperation = TableOperation.Delete(item);
                // Execute the operation.
                table.Execute(deleteOperation);
            }
        }

        public async Task StoreSupportGroupsMmembers(IEnumerable<SupportGroup> groups)
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(SupportGroupMembersTable);
            //truncate
            await table.CreateIfNotExistsAsync().ConfigureAwait(true);
            DeleteItems<SupportGroupMember>(table);
            table.CreateIfNotExists();
            var operation = new TableBatchOperation();
            var partitionKey = Guid.NewGuid().ToString();
            foreach (var supportGroup in groups)
            {
                var members =
                    supportGroup.Members.Select(
                        m => new SupportGroupMember(supportGroup.Name, supportGroup.Country, m.Name, partitionKey));
                foreach (var supportGroupMember in members)
                {
                    operation.Insert(supportGroupMember);
                }

            }
            await table.ExecuteBatchAsync(operation).ConfigureAwait(true);
        }

        public IEnumerable<Dto.SupportGroup> GetSupportGroups()
        {
            return GetAllItems<Dto.SupportGroup>(SupportGroupTable);
        }

        private IEnumerable<T> GetAllItems<T>(string tableName) where T : TableEntity, new()
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            var query = table.CreateQuery<T>();
            return query.Execute();
        }

        public IEnumerable<SupportGroupMember> GetSupportGroupMembers()
        {
            return GetAllItems<SupportGroupMember>(SupportGroupMembersTable);
        }

        public IEnumerable<BusinessService> GetBusinessServices()
        {
            return GetAllItems<BusinessService>(BusinessServiceTable);
        }

        public IEnumerable<ServicePortalRequest> GetServiceCatalogRequests()
        {
            return GetAllItems<Dto.ServicePortalRequest>(ServicePortalRequestTable);
        }
    }
}
