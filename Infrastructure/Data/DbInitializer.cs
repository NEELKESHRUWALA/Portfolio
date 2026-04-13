using Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedData(AppDbContext context, IConfiguration config)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Projects.AnyAsync())
            return;

        var portfolioData = config.GetSection("PortfolioData").Get<PortfolioDataConfig>();
        if (portfolioData is null)
            return;

        context.Projects.AddRange(portfolioData.Projects ?? []);
        context.Experiences.AddRange(portfolioData.Experiences ?? []);
        context.Skills.AddRange(portfolioData.Skills ?? []);
        context.Educations.AddRange(portfolioData.Educations ?? []);
        context.Certifications.AddRange(portfolioData.Certifications ?? []);

        await context.SaveChangesAsync();
    }

    private sealed class PortfolioDataConfig
    {
        public List<Project>? Projects { get; init; }
        public List<Experience>? Experiences { get; init; }
        public List<Skill>? Skills { get; init; }
        public List<Education>? Educations { get; init; }
        public List<Certification>? Certifications { get; init; }
    }
}