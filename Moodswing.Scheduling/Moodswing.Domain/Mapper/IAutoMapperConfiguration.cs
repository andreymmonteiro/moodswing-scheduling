using AutoMapper;

namespace Moodswing.Domain.Mapper
{
    public interface IAutoMapperConfiguration
    {
        IMapper CreateMapper();
    }
}
