using Portfolio.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Portfolio.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedData(AppDbContext context, IConfiguration config)
    {
        // Only create database if it doesn't exist (don't delete on every restart)
        await context.Database.EnsureCreatedAsync();

        // Check if data already exists to avoid duplicates
        if (await context.PersonalInfos.AnyAsync())
            return;

        var personalInfo = config.GetSection("PersonalInfo").Get<PersonalInfo>();
        var portfolioData = config.GetSection("PortfolioData").Get<PortfolioDataConfig>();

        if (personalInfo is null && portfolioData is null)
            return;

        if (personalInfo != null)
        {
            context.PersonalInfos.Add(personalInfo);
            await context.SaveChangesAsync();
        }

        if (portfolioData != null)
        {
            if (portfolioData.Projects != null)
            {
                foreach (var p in portfolioData.Projects)
                {
                    context.Projects.Add(new Project
                    {
                        Title = p.Title,
                        ShortDescription = p.ShortDescription,
                        FullDescription = p.FullDescription,
                        Challenges = p.Challenges,
                        Stack = p.Stack,
                        Icon = p.Icon,
                        Handling = p.Handling,
                        RecruiterPerspective = p.RecruiterPerspective
                    });
                }
            }

            if (portfolioData.Experiences != null)
            {
                foreach (var e in portfolioData.Experiences)
                {
                    context.Experiences.Add(new Experience
                    {
                        JobTitle = e.JobTitle,
                        Company = e.Company,
                        Period = e.Period,
                        Description = e.Description
                    });
                }
            }

            if (portfolioData.Skills != null)
            {
                foreach (var s in portfolioData.Skills)
                {
                    context.Skills.Add(new Skill
                    {
                        Name = s.Name,
                        Category = s.Category,
                        Proficiency = s.Proficiency
                    });
                }
            }

            if (portfolioData.Educations != null)
            {
                foreach (var ed in portfolioData.Educations)
                {
                    context.Educations.Add(new Education
                    {
                        Degree = ed.Degree,
                        Institution = ed.Institution,
                        Period = ed.Period,
                        Details = ed.Details
                    });
                }
            }

            if (portfolioData.Certifications != null)
            {
                foreach (var c in portfolioData.Certifications)
                {
                    context.Certifications.Add(new Certification
                    {
                        Name = c.Name,
                        Issuer = c.Issuer
                    });
                }
            }

            await context.SaveChangesAsync();
        }
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