using Portfolio.Domain.Entities;
using Portfolio.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Portfolio.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly AppDbContext _context;
    private static readonly ConcurrentDictionary<string, object> _cache = new();

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

        var profileDto = await _context.PersonalInfos.AsNoTracking().FirstOrDefaultAsync(ct);
        var profile = profileDto != null
            ? new PersonalInfo(profileDto.FirstName, profileDto.LastName, profileDto.Title, profileDto.Description, profileDto.GithubUrl, profileDto.LinkedinUrl, profileDto.Email)
            : new PersonalInfo("", "", "", "", "", "", "");

        return new PortfolioData(profile, projects, experiences, skills, educations, certifications);
    }

    private async Task<T> GetOrSetCacheAsync<T>(string key, Func<Task<T>> factory, CancellationToken ct) where T : class
    {
        if (_cache.TryGetValue(key, out var cached) && cached is T result)
            return result;

        result = await factory();
        _cache.TryAdd(key, result);
        return result;
    }

    public static void ClearCache() => _cache.Clear();
}
