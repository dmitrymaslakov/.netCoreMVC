using NewsGatheringService.DAL.Entities;
using System.Text.Json.Serialization;

namespace NewsGatheringService.Models.BLL
{
    public class AuthenticateResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public AuthenticateResponse(User user, string jwtToken, string refreshToken)
        {
            Id = user.Id.ToString("D");
            UserName = user.Login;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }

    }
}
