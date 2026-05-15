using Cwiczenia7.Data;
using Cwiczenia7.DTOs;
using Cwiczenia7.Entities;
using Cwiczenia7.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Cwiczenia7.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcDto>> GetAllAsync()
    {
        return await _context.PCs
            .Select(pc => new PcDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<PcComponentsDto> GetComponentsAsync(int id)
    {
        var pc = await _context.PCs
            .Include(pc => pc.PcComponents)
                .ThenInclude(pcComponent => pcComponent.Component)
                .ThenInclude(component => component.ComponentManufacturer)
            .Include(pc => pc.PcComponents)
                .ThenInclude(pcComponent => pcComponent.Component)
                .ThenInclude(component => component.ComponentType)
            .FirstOrDefaultAsync(pc => pc.Id == id);

        if (pc is null)
            throw new NotFoundException($"PC with id {id} not found");

        return new PcComponentsDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock,
            Components = pc.PcComponents.Select(pcComponent => new PcComponentDto
            {
                Amount = pcComponent.Amount,
                Component = new ComponentDto
                {
                    Code = pcComponent.Component.Code,
                    Name = pcComponent.Component.Name,
                    Description = pcComponent.Component.Description,
                    Manufacturer = new ManufacturerDto
                    {
                        Id = pcComponent.Component.ComponentManufacturer.Id,
                        Abbreviation = pcComponent.Component.ComponentManufacturer.Abbreviation,
                        FullName = pcComponent.Component.ComponentManufacturer.FullName,
                        FoundationDate = pcComponent.Component.ComponentManufacturer.FoundationDate
                    },
                    Type = new ComponentTypeDto
                    {
                        Id = pcComponent.Component.ComponentType.Id,
                        Abbreviation = pcComponent.Component.ComponentType.Abbreviation,
                        Name = pcComponent.Component.ComponentType.Name
                    }
                }
            }).ToList()
        };
    }

    public async Task<PcDto> CreateAsync(CreatePcDto dto)
    {
        var pc = new Pc
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return MapPcToDto(pc);
    }

    public async Task<PcDto> UpdateAsync(int id, CreatePcDto dto)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
            throw new NotFoundException($"PC with id {id} not found");

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();

        return MapPcToDto(pc);
    }

    public async Task DeleteAsync(int id)
    {
        var pc = await _context.PCs.FindAsync(id);

        if (pc is null)
            throw new NotFoundException($"PC with id {id} not found");

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
    }

    private static PcDto MapPcToDto(Pc pc)
    {
        return new PcDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }
}