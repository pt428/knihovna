using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
    public class EmployeeService
    {
        private ApplicationDbContext _dbContext;

        public EmployeeService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        //*******************************
        //********* CREATE   ************
        //*******************************
        public async Task CreateAsync(EmployeeDto employeeDto)
        {
            await _dbContext.Employees.AddAsync(DtoToModel(employeeDto));
            await _dbContext.SaveChangesAsync();
        }
        //*******************************
        //********* READ  ************
        //*******************************
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var allEmployees = await _dbContext.Employees.ToListAsync();
            var employeesDtos = new List<EmployeeDto>();
            foreach (var employee in allEmployees)
            {
                employeesDtos.Add(ModelToDto(employee));
            }
            return employeesDtos;
        }
        //*******************************
        //********* UPDATE  ************
        //*******************************
        public async Task<EmployeeDto> EditAsync(EmployeeDto employeeDto)
        {
            _dbContext.Update(DtoToModel(employeeDto));
            await _dbContext.SaveChangesAsync();
            return employeeDto;
        }
        //*******************************
        //********* DELETE  ************
        //*******************************
        public async Task DeleteAsync(int id)
        {
            var employeeToDelete = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
                        _dbContext.Remove(employeeToDelete);
            await _dbContext.SaveChangesAsync();
        }
        //*******************************
        //********* GET BY ID  ************
        //*******************************
        public async Task<EmployeeDto> GetByIdAsync(int id)
        {
            var employeeToDelete = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return ModelToDto(employeeToDelete);

        }

        //*******************************
        //********* MODEL TO DTO  ************
        //*******************************
        public EmployeeDto ModelToDto(Employee employee)
        {
            return new EmployeeDto()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth
            };
        }
        //*******************************
        //********* DTO TO MODEL  ************
        //*******************************
        public Employee DtoToModel(EmployeeDto employeeDto)
        {
            return new Employee()
            {
                Id = employeeDto.Id,
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                DateOfBirth = employeeDto.DateOfBirth
            };
        }
    }
}
