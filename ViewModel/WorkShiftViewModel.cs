namespace WebApplication1.ViewModel
{
    public class WorkShiftViewModel
    {
        public string Name { get; set; }

        // Recebemos como string "07:00" e convertemos para TimeSpan
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public int ExpectedHours { get; set; }
    }
}
