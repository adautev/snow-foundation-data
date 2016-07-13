using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using ITCE.SNOW.Data.AzureDataTable;
using ITCE.SNOW.Data.ServiceNow;
using ITCE.SNOW.Data.ServiceNow.ServiceNow.Configuration;
using ITCE.SNOW.Domain;
using ITCE.SNOW.Domain.Validation;
using ITCE.SNOW.Domain.Validation.Concrete;

namespace ITCE.SNOW.Services
{
    public class ValidationService
    {
        private readonly ServiceNowRepository _serviceNowRepository;
        private List<User> _serviceNowUsers;
        private List<BusinessService> _serviceNowBusinessServices;
        private List<ServicePortalRequest> _serviceNowRequests;
        private List<SupportGroup> _serviceNowSupportGroups;

        public IValidationSummary<SupportGroup> ValidateSupportGroups(IEnumerable<SupportGroup> groups)
        {
            var summary = new SupportGroupValidationSummary()
            {
                Name = "Support Groups"
            };
            //General compliance
            var supportGroupValidator = new SupportGroupValidator();
            var basicValidation = groups.Select(g => new KeyValuePair<SupportGroup, ValidationResult>(g, supportGroupValidator.Validate(g)));
            if (basicValidation.Any(v => !v.Value.IsValid))
            {
                summary.Messages.AddRange(basicValidation.Where(v => !v.Value.IsValid).Select(v => new ValidationMessage<SupportGroup>
                {
                    Reference = v.Key,
                    Message = v.Value.Errors.Select(e => e.ErrorMessage).Aggregate((previous, next) => $"{previous}<br/>{next}")
                }));
            }
            ValidateSupportGroupManagerCompliance(summary, groups, _serviceNowUsers);
            ValidateSupportGroupMembersCompliance(summary, groups, _serviceNowUsers);
            return summary;

        }
        public IValidationSummary<BusinessService> ValidateBusinessServices(IEnumerable<BusinessService> services, IEnumerable<SupportGroup> supportGroups)
        {
            var summary = new BusinessServiceValidationSummary()
            {
                Name = "Business Services"
            };
            //General compliance
            CheckBusinessServiceGeneralCompliance(services, summary);
            ValidateBusinessServiceManagerCompliance(summary, services, _serviceNowUsers);
            ValidateBusinessServiceAssignmentGroups(summary, services, _serviceNowSupportGroups, supportGroups);
            return summary;

        }

        private static void CheckBusinessServiceGeneralCompliance(IEnumerable<BusinessService> services, BusinessServiceValidationSummary summary)
        {
            var validator = new BusinessServiceValidator();
            var basicValidation =
                services.Select(g => new KeyValuePair<BusinessService, ValidationResult>(g, validator.Validate(g)));
            if (basicValidation.Any(v => !v.Value.IsValid))
            {
                summary.Messages.AddRange(
                    basicValidation.Where(v => !v.Value.IsValid).Select(v => new ValidationMessage<BusinessService>
                    {
                        Reference = v.Key,
                        Message =
                            v.Value.Errors.Select(e => e.ErrorMessage).Aggregate((previous, next) => $"{previous}<br/>{next}")
                    }));
            }
        }

        public IValidationSummary<ServicePortalRequest> ValidateServiceCatalogRequests(IEnumerable<ServicePortalRequest> requests, IEnumerable<SupportGroup> supportGroups, IEnumerable<BusinessService> businessServices)
        {
            var summary = new ServicePortalRequestValidationSummary()
            {
                Name = "ServicePortalRequests"
            };
            //General compliance
            var validator = new ServicePortalRequestValidator();
            var basicValidation = requests.Select(g => new KeyValuePair<ServicePortalRequest, ValidationResult>(g, validator.Validate(g)));
            if (basicValidation.Any(v => !v.Value.IsValid))
            {
                summary.Messages.AddRange(basicValidation.Where(v => !v.Value.IsValid).Select(v => new ValidationMessage<ServicePortalRequest>
                {
                    Reference = v.Key,
                    Message = v.Value.Errors.Select(e => e.ErrorMessage).Aggregate((previous, next) => $"{previous}<br/>{next}")
                }));
            }
            ValidateServicePortalRequestRelatedBusinessService(summary, requests, businessServices, _serviceNowBusinessServices);
            ValidateServicePortalRequestImplementerGroup(summary, requests, supportGroups, _serviceNowSupportGroups);
            return summary;

        }

        private void ValidateServicePortalRequestImplementerGroup(ServicePortalRequestValidationSummary summary, IEnumerable<ServicePortalRequest> requests, IEnumerable<SupportGroup> supportGroups, List<SupportGroup> serviceNowSupportGroups)
        {
            var messages = requests.Where(
                r =>
                    serviceNowSupportGroups.All(sg => sg.Name != r.AssignmentGroup) &&
                    supportGroups.All(sg => sg.Name != r.AssignmentGroup))
                .Select(sp => new ValidationMessage<ServicePortalRequest>
                {
                    Message =
                        $"No implementer group \"{sp.AssignmentGroup}\" found in ServiceNow nor in the business services sheet for request {sp.Name}.",
                    Reference = sp
                }).ToList();
            if (messages.Count > 0)
            {
                summary.Messages.AddRange(messages);
            }
        }

        private void ValidateBusinessServiceAssignmentGroups(BusinessServiceValidationSummary summary, IEnumerable<BusinessService> services, List<SupportGroup> serviceNowSupportGroups, IEnumerable<SupportGroup> supportGroups)
        {
            var messages = services.Where(
                s =>
                    serviceNowSupportGroups.All(sg => sg.Name != s.AssignmentGroup) &&
                    supportGroups.All(sg => sg.Name != s.AssignmentGroup))
                .Select(sg => new ValidationMessage<BusinessService>
                {
                    Message =
                        $"No assignment group \"{sg.AssignmentGroup}\" found in ServiceNow nor in the support groups sheet for business service {sg.Name}.",
                    Reference = sg
                }).ToList();
            if (messages.Count > 0)
            {
                summary.Messages.AddRange(messages);
            }
        }

        /// <summary>
        /// Checks whether the defined business businessServices for the businessServices can be found in SNOW or in the import source we are validating.
        /// </summary>
        /// <param name="summary">The validation summary that will contain all messages.</param>
        /// <param name="businessServices">A list of import source businessServices to validate against.</param>
        /// <param name="serviceNowBusinessServices">All business services present in ServiceNow</param>
        private void ValidateServicePortalRequestRelatedBusinessService(ServicePortalRequestValidationSummary summary, IEnumerable<ServicePortalRequest> requests, IEnumerable<BusinessService> businessServices, List<BusinessService> serviceNowBusinessServices)
        {
            var messages = requests.Where(
                r =>
                    serviceNowBusinessServices.All(sg => sg.Name != r.BusinessService) &&
                    businessServices.All(sg => sg.Name != r.BusinessService))
                .Select(sp => new ValidationMessage<ServicePortalRequest>
                {
                    Message =
                        $"No business service \"{sp.BusinessService}\" found in ServiceNow nor in the business services sheet for request {sp.Name}.",
                    Reference = sp
                }).ToList();
            if (messages.Count > 0)
            {
                summary.Messages.AddRange(messages);
            }
        }

        private void ValidateSupportGroupMembersCompliance(SupportGroupValidationSummary summary, IEnumerable<SupportGroup> groups, List<User> serviceNowUsers)
        {

            foreach (var supportGroup in groups)
            {
                foreach (var member in supportGroup.Members.Where(member => serviceNowUsers.All(u => u.Name != member.Name)))
                {
                    summary.Messages.Add(new ValidationMessage<SupportGroup>
                    {
                        Message = $"Member {member.Name} not  defined for support group: {supportGroup.Name}",
                        Reference = supportGroup
                    });
                }
            }
        }

        private void ValidateSupportGroupManagerCompliance(IValidationSummary<SupportGroup> summary, IEnumerable<SupportGroup> groups, List<User> serviceNowUsers)
        {
            summary.Messages.AddRange(groups.Where(g => g.Manager == null)
                .Select(g => new ValidationMessage<SupportGroup>
                {
                    Message = $"Manager not  defined for support group: {g.Name}",
                    Reference = g
                }));
            summary.Messages.AddRange(groups.Where(g => g.Manager != null && serviceNowUsers.All(su => su.Name != g.Manager.Name))
                .Select(g => new ValidationMessage<SupportGroup>
                {
                    Message = $"Manager not found in ServiceNow: {g.Manager.Name}",
                    Reference = g
                }));
        }
        private void ValidateBusinessServiceManagerCompliance(IValidationSummary<BusinessService> summary, IEnumerable<BusinessService> Services, List<User> serviceNowUsers)
        {

            summary.Messages.AddRange(Services.Where(g => g.Manager != null && serviceNowUsers.All(su => su.Name != g.Manager.Name))
                .Select(g => new ValidationMessage<BusinessService>
                {
                    Message = $"Manager not found in ServiceNow: {g.Manager.Name}",
                    Reference = g
                }));
        }

        public ValidationService(ServiceNowConfiguration serviceNowSettings)
        {
            _serviceNowRepository = new ServiceNowRepository(serviceNowSettings);
            //Lame
            Task.WaitAll(GetUsers(), GetBusinessServices(), GetCategories(), GetSupportGroups());
        }
        private async Task GetCategories()
        {
            _serviceNowRequests = await _serviceNowRepository.GetRequests().ConfigureAwait(true);
        }

        private async Task GetBusinessServices()
        {
            _serviceNowBusinessServices = await _serviceNowRepository.GetBusinessServices().ConfigureAwait(true);
        }

        private async Task GetUsers()
        {
            _serviceNowUsers = await _serviceNowRepository.GetUsers().ConfigureAwait(true);
        }
        private async Task GetSupportGroups()
        {
            _serviceNowSupportGroups = await _serviceNowRepository.GetSupportGroups().ConfigureAwait(true);
        }
    }
}

