namespace noter.Entities
{
    public class Note
    {
        public long Id { get; set; }
        public string Payload { get; set; }

        public Note()
        {
            
        }
        public Note(string payload)
        {
            Payload = payload;
        }
    }
}
