using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SPJApiPublica.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using SPJApiPublica.ViewModel;

namespace SPJApiPublica.Services
{
    public class CarRepository
    {
        private readonly AppDbContext _context;

        public CarRepository()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public async Task AddCarAsync(Car car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task UpdateCarAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCarAsync(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}
