using task_2.Models;
using AutoMapper;

namespace task_2.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NotificationEntity, Notification>();
        }
    }
}