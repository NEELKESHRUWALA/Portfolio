using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Portfolio.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _context;
    private static readonly ConcurrentDictionary<string, object> _cache = new();
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public PortfolioService(AppDbContext context) => _context = context;

    public async Task<List<Project>> GetProjectsAsync(CancellationToken ct = default)
        => await GetOrSetCacheAsync("projects", () => _context.Projects.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Experience>> GetExperiencesAsync(CancellationToken ct = default)
        => await GetOrSetCacheAsync("experiences", () => _context.Experiences.AsNoTracking().OrderByDescending(e => e.Id).ToListAsync(ct), ct);

    public async Task<List<Skill>> GetSkillsAsync(CancellationToken ct = default)
        => await GetOrSetCacheAsync("skills", () => _context.Skills.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Education>> GetEducationsAsync(CancellationToken ct = default)
        => await GetOrSetCacheAsync("educations", () => _context.Educations.AsNoTracking().ToListAsync(ct), ct);

    public async Task<List<Certification>> GetCertificationsAsync(CancellationToken ct = default)
        => await GetOrSetCacheAsync("certifications", () => _context.Certifications.AsNoTracking().ToListAsync(ct), ct);

    public async Task<PortfolioData> GetAllDataAsync(CancellationToken ct = default)
    {
        var projects = await GetProjectsAsync(ct);
        var experiences = await GetExperiencesAsync(ct);
        var skills = await GetSkillsAsync(ct);
        var educations = await GetEducationsAsync(ct);
        var certifications = await GetCertificationsAsync(ct);

        var profileDto = await GetOrSetCacheAsync("profile", () => _context.PersonalInfos.AsNoTracking().FirstOrDefaultAsync(ct), ct);
        var profile = profileDto != null
            ? profileDto
            : new PersonalInfo { FirstName = "", LastName = "", Title = "", Description = "", GithubUrl = "", LinkedinUrl = "", Email = "" };

        return new PortfolioData(profile, projects, experiences, skills, educations, certifications);
    }

    private async Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> factory, CancellationToken ct) where T : class
    {
        if (_cache.TryGetValue(key, out var cached) && cached is T result1)
            return result1;

        await _semaphore.WaitAsync(ct);
        try
        {
            if (_cache.TryGetValue(key, out var cachedInside) && cachedInside is T result2)
                return result2;

            var result = await factory();
            if (result != null)
            {
                _cache.TryAdd(key, result);
            }
            return result!;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public static void ClearCache() => _cache.Clear();
}
