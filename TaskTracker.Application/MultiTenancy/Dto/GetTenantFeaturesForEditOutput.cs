using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TaskTracker.Editions.Dto;

namespace TaskTracker.MultiTenancy.Dto
{
    public class GetTenantFeaturesForEditOutput : IOutputDto
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}