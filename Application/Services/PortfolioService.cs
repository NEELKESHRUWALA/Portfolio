using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Portfolio.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _context;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public PortfolioService(AppDbContext context) => _context = context;

    public async Task<List<Project>> GetProjectsAsync(CancellationToken ct = default)
        => await ExecuteSafeAsync(() => _context.Projects.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Experience>> GetExperiencesAsync(CancellationToken ct = default)
        => await ExecuteSafeAsync(() => _context.Experiences.AsNoTracking().OrderByDescending(e => e.Id).ToListAsync(ct), ct);

    public async Task<List<Skill>> GetSkillsAsync(CancellationToken ct = default)
        => await ExecuteSafeAsync(() => _context.Skills.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Education>> GetEducationsAsync(CancellationToken ct = default)
        => await ExecuteSafeAsync(() => _context.Educations.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Certification>> GetCertificationsAsync(CancellationToken ct = default)
        => await ExecuteSafeAsync(() => _context.Certifications.AsNoTracking().ToListAsync(ct), ct);

    public async Task<PortfolioData> GetAllDataAsync(CancellationToken ct = default)
    {
        var projects = await GetProjectsAsync(ct);
        var experiences = await GetExperiencesAsync(ct);
        var skills = await GetSkillsAsync(ct);
        var educations = await GetEducationsAsync(ct);
        var certifications = await GetCertificationsAsync(ct);

        var profileDto = await ExecuteSafeAsync(() => _context.PersonalInfos.AsNoTracking().FirstOrDefaultAsync(ct), ct);
        var profile = profileDto != null
            ? profileDto
            : new PersonalInfo { FirstName = "", LastName = "", Title = "", Description = "", GithubUrl = "", LinkedinUrl = "", Email = "" };

        return new PortfolioData(profile, projects, experiences, skills, educations, certifications);
    }

    private async Task<T> ExecuteSafeAsync<T>(Func<Task<T>> factory, CancellationToken ct)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            return await factory();
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
