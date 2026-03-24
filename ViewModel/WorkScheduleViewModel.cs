namespace WebApplication1.ViewModel
{
    public class WorkScheduleViewModel
    {
        public int EmployeeId { get; set; }
        public int WorkShiftId { get; set; }

        // Data no formato "2025-03-24"
        public DateOnly Date { get; set; }

        public string? Note { get; set; }
    }
}
