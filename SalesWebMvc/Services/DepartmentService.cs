using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private readonly SalesWebMvcContext _context;

        public DepartmentService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //Chamada síncrona do banco de dados 
        //public List<Department> FindAll()
        //{
        //    return _context.Department.OrderBy(x => x.Name).ToList();
        //}

        //Chamada assíncrona
        public async Task<List<Department>> FindAllAsync()
        {
            //O ToList() é uma operação assíncrona. Por isso estou usando o
            //ToListAsync() que pertence ao EntityFramework
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }
    }
}
