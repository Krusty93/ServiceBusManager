using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using ServiceBusManager.Server.Application.Queries;
using ServiceBusManager.Server.Providers.Azure.Models;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Application.Mappers
{
    public class QueueMapper : Profile
    {
        public QueueMapper()
        {
            CreateMap<ServiceBusQueueStatus, ServiceBusQueueStatusResponse>()
                .ConvertUsingEnumMapping();

            CreateMap<AzureServiceBusQueue, QueueGetResponse>();

            CreateMap<AzureServiceBusQueueDetails, QueueGetDetailsResponse>()
                .ForMember(dest => dest.DefaultMessageTimeToLive, opt =>
                    opt.MapFrom(src => src.MessageSettings.DefaultMessageTimeToLive))
                .ForMember(dest => dest.LockDuration, opt =>
                    opt.MapFrom(src => src.MessageSettings.LockDuration))
                .ForMember(dest => dest.AutoDeleteOnIdle, opt =>
                    opt.MapFrom(src => src.MessageSettings.AutoDeleteOnIdle))
                .ForMember(dest => dest.RequireSession, opt =>
                    opt.MapFrom(src => src.Settings.RequireSession))
                .ForMember(dest => dest.EnablePartitioning, opt =>
                    opt.MapFrom(src => src.Settings.EnablePartitioning))
                .ForMember(dest => dest.EnableBatchedOperations, opt =>
                    opt.MapFrom(src => src.Settings.EnableBatchedOperations))
                .ForMember(dest => dest.RequireDuplicateDetection, opt =>
                    opt.MapFrom(src => src.Settings.RequireDuplicateDetection))
                .ForMember(dest => dest.DuplicateDetectionHistoryTimeWindow, opt =>
                    opt.MapFrom(src => src.Properties.DuplicateDetectionHistoryTimeWindow))
                .ForMember(dest => dest.MaxDeliveryCount, opt =>
                    opt.MapFrom(src => src.Properties.MaxDeliveryCount))
                .ForMember(dest => dest.MaxSizeInMegabytes, opt =>
                    opt.MapFrom(src => src.Properties.MaxSizeInMegabytes));
        }
    }
}
