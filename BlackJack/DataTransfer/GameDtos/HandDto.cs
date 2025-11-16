namespace BlackJack.DataTransfer.GameDtos
{
    public class HandDto
    {
        public List<string> CardKeys { get; set; } = new();
        public bool IsDealer { get; set; }
    }
}
