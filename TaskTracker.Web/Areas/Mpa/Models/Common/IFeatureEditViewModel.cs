using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TaskTracker.Editions.Dto;

namespace TaskTracker.Web.Areas.Mpa.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}