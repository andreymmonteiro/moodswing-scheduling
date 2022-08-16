namespace Moodswing.Domain.Models.Authentication
{
    public struct Token
    {
        private readonly string _token;

        private Token(string token)
            => _token = token;

        public static implicit operator Token(string token)
            => new(token);
        
        public string GetToken
            => _token;
    }
}
