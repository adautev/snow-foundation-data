using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using ITCE.SNOW.Data.AzureDataTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ITCE.SNOW.FoundationData.Controllers
{
    public class ExportController : Controller
    {
        private readonly DataTableStorageRepository _dataRepository;

        public ExportController(IOptions<DataTableSettings> dataTableSettings)
        {
            //This is dirty, but DI will come later
            _dataRepository = new DataTableStorageRepository(dataTableSettings.Value);
        }

        public async Task<FileResult> SupportGroups()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    var csvWriter = new CsvWriter(writer)
                    {
                        Configuration =
                        {
                            HasHeaderRecord = true,
                            Encoding = Encoding.UTF8
                        }
                    };
                    var supportGroups = _dataRepository.GetSupportGroups().ToList();
                    csvWriter.WriteRecords(supportGroups);
                    writer.Flush();

                    var fileBytes = stream.ToArray();
                    const string fileName = "supportGroups.csv";
                    writer.Close();
                    stream.Close();
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
        }
        public async Task<FileResult> SupportGroupMembers()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    var csvWriter = new CsvWriter(writer)
                    {
                        Configuration =
                        {
                            HasHeaderRecord = true,
                            Encoding = Encoding.UTF8
                        }
                    };
                    var members = _dataRepository.GetSupportGroupMembers().ToList();
                    csvWriter.WriteRecords(members);
                    writer.Flush();

                    var fileBytes = stream.ToArray();
                    const string fileName = "supportGroupMembers.csv";
                    writer.Close();
                    stream.Close();
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
        }
        public async Task<FileResult> BusinessServices()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    var csvWriter = new CsvWriter(writer)
                    {
                        Configuration =
                        {
                            HasHeaderRecord = true,
                            Encoding = Encoding.UTF8
                        }
                    };
                    var services = _dataRepository.GetBusinessServices().ToList();
                    csvWriter.WriteRecords(services);
                    writer.Flush();

                    var fileBytes = stream.ToArray();
                    const string fileName = "businessServices.csv";
                    writer.Close();
                    stream.Close();
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
        }
        public async Task<FileResult> ServiceCatalogRequests()
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    var csvWriter = new CsvWriter(writer)
                    {
                        Configuration =
                        {
                            HasHeaderRecord = true,
                            Encoding = Encoding.UTF8
                        }
                    };
                    var requests = _dataRepository.GetServiceCatalogRequests().ToList();
                    csvWriter.WriteRecords(requests);
                    writer.Flush();

                    var fileBytes = stream.ToArray();
                    const string fileName = "serviceCatalogRequests.csv";
                    writer.Close();
                    stream.Close();
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
        }
    }
}
