namespace Explorer.Stakeholders.API.Dtos
{
    public class SocialProfileDto
    {
        public long userId { get; set; }
        public string username { get; set; }
        public List<SocialProfileDto> followers { get; set; }
        public List<SocialProfileDto> following { get; set; }
    }
}
