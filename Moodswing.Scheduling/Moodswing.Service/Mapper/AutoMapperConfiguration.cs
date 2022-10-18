using AutoMapper;
using Moodswing.Domain.Mapper;

namespace Moodswing.Service.Mapper
{
    public sealed class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        public IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(option => 
            {
                option.AddProfile(new EntityToDto());
            });

            return mapper.CreateMapper();
        }
    }
}
