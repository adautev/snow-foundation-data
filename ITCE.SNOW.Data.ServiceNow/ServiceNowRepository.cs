using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ITCE.SNOW.Data.ServiceNow.ServiceNow;
using ITCE.SNOW.Data.ServiceNow.ServiceNow.Configuration;
using ITCE.SNOW.Data.ServiceNow.ServiceNow.Dto;
using ITCE.SNOW.Domain;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ITCE.SNOW.Data.ServiceNow
{
    public class ServiceNowRepository
    {
        private readonly string _userName = "andrey.dautev@itce.com";
        private readonly string _password = "ad1.ad1";
        private readonly string _instanceUrl = "https://titandev.service-now.com/api/now/table/";

        public ServiceNowRepository(ServiceNowConfiguration settings)
        {
           
            if (settings == null)
            {
                throw new ArgumentException("The ServiceNowConfigurationSection is not defined");
            }
            _instanceUrl = settings.InstanceUrl;
            _userName = settings.Username;
            _password = settings.Password;
        }
        public async Task<List<ServicePortalRequest>> GetRequests()
        {
            var client = ConfigureRestClient();
            //client.AddHandler("applicationdynamic/json", new DynamicJsonDeserializer());
            var request = new RestRequest($"sc_cat_item?{new ServicePortalRequestDto().Fields}&sysparm_query=active=true", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteGetTaskAsync<RestApiResponse>(request).ConfigureAwait(true);
            return JsonConvert.DeserializeObject<List<ServicePortalRequestDto>>(response.Data.Result).Cast<ServicePortalRequest>().ToList();
        }

        private RestClient ConfigureRestClient()
        {
            var client = new RestClient(_instanceUrl)
            {
                Authenticator = new HttpBasicAuthenticator(_userName, _password)
            };
            return client;
        }

        public async Task<List<BusinessService>> GetBusinessServices()
        {
            var client = ConfigureRestClient();
            var request = new RestRequest($"sc_cat_item?{new BusinessServiceDto().Fields}&sysparm_query=active=true", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteGetTaskAsync<RestApiResponse>(request).ConfigureAwait(true);
            return JsonConvert.DeserializeObject<List<BusinessServiceDto>>(response.Data.Result).Cast<BusinessService>().ToList();
        }

        public async Task<List<User>> GetUsers()
        {
            var client = ConfigureRestClient();
            var request = new RestRequest($"sys_user?{new UserDto().Fields}&sysparm_query=active=true", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteGetTaskAsync<RestApiResponse>(request).ConfigureAwait(true);
            return JsonConvert.DeserializeObject<List<UserDto>>(response.Data.Result).Cast<User>().ToList();
        }

        public async Task<List<SupportGroup>>  GetSupportGroups()
        {
            var client = ConfigureRestClient();
            var request = new RestRequest($"sys_user?{new SupportGroupDto().Fields}&sysparm_query=active=true", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = await client.ExecuteGetTaskAsync<RestApiResponse>(request).ConfigureAwait(true);
            return JsonConvert.DeserializeObject<List<SupportGroupDto>>(response.Data.Result).Cast<SupportGroup>().ToList();
        }
    }
}
