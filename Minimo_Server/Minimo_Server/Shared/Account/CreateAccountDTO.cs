namespace MinimoShared
{
    /// <summary>
    /// 계정 생성 DTO. 유저네임, 패스워드, 닉네임을 담고 있다.
    /// </summary>
    public class CreateAccountDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}