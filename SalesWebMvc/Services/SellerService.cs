using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        //public List<Seller> FindAll()
        //{
        //    return _context.Seller.ToList();
        //}

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        //Insert síncrono
        //public void Insert(Seller obj)
        //{
        // _context.Add(obj);
        //_context.SaveChanges();
        //}

        //Insert Assíncrono
        public async Task InsertAsync(Seller obj)
        {
            //A operação Add() é executada em memória
            _context.Add(obj);

            //A operação SaveChanges é quem realmente acessa o banco de dados
            await _context.SaveChangesAsync();
        }



        //public Seller FindById(int id)
        //{
        //    //O Método Include é utlizado para fazer o join na tabela de departamentos
        //   return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        //}

        public async Task<Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }

        //public void Remove(int id)
        //{
        //    var obj = _context.Seller.Find(id);
        //    _context.Seller.Remove(obj);
        //    _context.SaveChanges();
        //}

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
        }

        //public void Update(Seller obj)
        //{
        //    testo se existe um vendedor com esse ID cadastrado no BD
        //    if (!_context.Seller.Any(x => x.Id == obj.Id))
        //    {
        //        throw new NotFoundException("Id Not Found");
        //    }
        //    try
        //    {
        //        _context.Update(obj);
        //        _context.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException e)
        //    {
        //        Capturo a excessão do nível de acesso a dados e lanço-a a nivel de serviço
        //        throw new DbUpdateConcurrencyException(e.Message);
        //    }
        //}

        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id Not Found");
            }
            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateConcurrencyException(e.Message);
            }
        }
    }
}
