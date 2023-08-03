using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller
    {
        public int Id { get; set; }

        //Required do HTML e min/max lenght com mensagem de erro personalizada
        //{0} = Pega o Name / {2} = Qtd min de caracteres / {1} = Qtd max de caracteres
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} size should be between {2} and {1}")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        //O framework pega o nome da classe para exibir no front. Usando a linha abaixo eu defino um nome para ser
        //exibido ao invés do nome da classe. O DataType é utilizado para tirar as horas dos campos de data, deixando apenas dd/MM/yyyy
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Range(100.00, 50000.00, ErrorMessage = "{0} must be from {1} to {2}")]
        [Display(Name = "Base Salary")]
        //Display format é para formatar o formato da string
        //{0:F2} onde 0 indica o valor do atributo onde este atriburo terá a formatação F2 (duas casas decimais)
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord record)
        {
            Sales.Add(record);
        }

        public void RemoveSales(SalesRecord record)
        {
            Sales.Remove(record);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr=> sr.Amount);
        }
    }
}
