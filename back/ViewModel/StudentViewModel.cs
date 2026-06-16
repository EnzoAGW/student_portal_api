namespace WebApplication1.ViewModel
{
    // ViewModel = o "formulário" que o usuário preenche para cadastrar um aluno
    // É separado do Model porque pode ter validações, campos opcionais, etc.
    public class StudentViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Enrollment { get; set; }
        public string Course { get; set; }
        public DateTime BirthDate { get; set; }

        // IFormFile = arquivo enviado pelo usuário (a foto)
        // O "?" significa que é opcional — aluno pode não ter foto
        public IFormFile? Photo { get; set; }
    }
}
