using Knihovna.DTO;
using Knihovna.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
    public class ReaderService
    {
        private ApplicationDbContext _dbContext;

        public ReaderService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //*******************************
        //********* CREATE   ************
        //*******************************
        public async Task CreateAsync(ReaderDto readerDto)
        {
            await _dbContext.Readers.AddAsync(DtoToModel(readerDto));
            await _dbContext.SaveChangesAsync();
        }
        //*******************************
        //********* READ  ************
        //*******************************
        public async Task<IEnumerable<ReaderDto>> GetAllAsync()
        {
            var allreaders = await _dbContext.Readers.ToListAsync();
            var readersDtos = new List<ReaderDto>();
            foreach (var reader in allreaders)
            {
                readersDtos.Add(ModelToDto(reader));
            }
            return readersDtos;
        }
        //*******************************
        //********* UPDATE  ************
        //*******************************
        public async Task<ReaderDto> EditAsync(ReaderDto readerDto)
        {
            _dbContext.Update(DtoToModel(readerDto));
            await _dbContext.SaveChangesAsync();
            return readerDto;
        }
        //*******************************
        //********* DELETE  ************
        //*******************************
        public async Task DeleteAsync(int id)
        {
            var readerToDelete = await _dbContext.Readers.FirstOrDefaultAsync(x => x.Id == id);
            _dbContext.Remove(readerToDelete);
            await _dbContext.SaveChangesAsync();
        }
        //*******************************
        //********* GET BY ID  ************
        //*******************************
        public async Task<ReaderDto> GetByIdAsync(int id)
        {
            var readerToDelete = await _dbContext.Readers.FirstOrDefaultAsync(x => x.Id == id);
            return ModelToDto(readerToDelete);

        }

        //*******************************
        //********* MODEL TO DTO  ************
        //*******************************
        public ReaderDto ModelToDto(Reader reader)
        {
            return new ReaderDto()
            {
                Id = reader.Id,
                FirstName = reader.FirstName,
                LastName = reader.LastName,
                DateOfBirth = reader.DateOfBirth
            };
        }
        //*******************************
        //********* DTO TO MODEL  ************
        //*******************************
        public Reader DtoToModel(ReaderDto readerDto)
        {
            return new Reader()
            {
                Id = readerDto.Id,
                FirstName = readerDto.FirstName,
                LastName = readerDto.LastName,
                DateOfBirth = readerDto.DateOfBirth
            };
        }
    }
}
