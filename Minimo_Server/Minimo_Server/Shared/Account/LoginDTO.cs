namespace MinimoShared
{
    /// <summary>
    /// 로그인 DTO. 유저네임, 패스워드를 담고 있다.
    /// </summary>
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}