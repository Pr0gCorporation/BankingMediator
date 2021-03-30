namespace Internship.Shared.Events
{
    public class OutgoingFileEvent
    {
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
