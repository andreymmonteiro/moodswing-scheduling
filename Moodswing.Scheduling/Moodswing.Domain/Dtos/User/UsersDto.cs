using Newtonsoft.Json;
using System.Collections.Generic;

namespace Moodswing.Domain.Dtos.User
{
    public sealed class UsersDto
    {
        [JsonProperty("users")]
        public IEnumerable<UserDto> Users { get; set; }
    }
}
