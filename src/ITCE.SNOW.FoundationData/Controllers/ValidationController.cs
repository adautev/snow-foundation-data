using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ITCE.SNOW.Data.AzureDataTable;
using ITCE.SNOW.Data.Excel;
using ITCE.SNOW.Data.ServiceNow.ServiceNow.Configuration;
using ITCE.SNOW.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ITCE.SNOW.FoundationData.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ValidationService _validationService;
        private readonly DataTableStorageRepository _dataRepository;

        public ValidationController(IOptions<ServiceNowConfiguration> serviceNowConfigurationOptions, IOptions<DataTableSettings> dataTableSettings)
        {
            //This is dirty, but DI will come later
            _validationService = new ValidationService(serviceNowConfigurationOptions.Value);
            _dataRepository = new DataTableStorageRepository(dataTableSettings.Value);
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ICollection<IFormFile> files)
        {
            var validationSummaries = new List<object>();
            var repository = new FoundationDataRepository();
            
            foreach (var file in files.Where(file => file.Length > 0))
            {
                using (var stream = file.OpenReadStream())
                {
                        
                    var groupsStream = new MemoryStream();
                    var businessServicesStream = new MemoryStream();
                    var requestsStream = new MemoryStream();

                    await CopyToAsync(stream, groupsStream).ConfigureAwait(true);
                    var groups = repository.GetSupportGroups(groupsStream);
                    await CopyToAsync(stream, businessServicesStream).ConfigureAwait(true);
                    var services = repository.GetBusinessServices(businessServicesStream);
                    await CopyToAsync(stream, requestsStream).ConfigureAwait(true);
                    var requests = repository.GetServicePortalRequests(requestsStream);
                    validationSummaries.Add(_validationService.ValidateSupportGroups(groups));
                    validationSummaries.Add(_validationService.ValidateBusinessServices(services,groups));
                    validationSummaries.Add(_validationService.ValidateServiceCatalogRequests(requests,groups,services));
                    ViewBag.ValidationSummaries = validationSummaries;
                    stream.Close();
                    await _dataRepository.StoreSupportGroups(groups).ConfigureAwait(true);
                    await _dataRepository.StoreSupportGroupsMmembers(groups).ConfigureAwait(true);
                    await _dataRepository.StoreBusinessServices(services).ConfigureAwait(true);
                    await _dataRepository.StoreServicePortalRequests(requests).ConfigureAwait(true);
                }
            }
            return View();
        }

        private async Task CopyToAsync(Stream stream, MemoryStream targetStream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(targetStream).ConfigureAwait(true);
            targetStream.Seek(0, SeekOrigin.Begin);
        }
    }
}
