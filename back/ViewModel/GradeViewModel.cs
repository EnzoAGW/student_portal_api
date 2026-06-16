namespace WebApplication1.ViewModel
{
    public class GradeViewModel
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }

        // A nota (ex: 8.5)
        public decimal Value { get; set; }

        // Tipo: "Prova 1", "Prova 2", "Trabalho Final"...
        public string EvaluationType { get; set; }

        public DateTime Date { get; set; }
    }
}
