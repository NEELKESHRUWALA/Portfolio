namespace Portfolio.Application.Services;

public interface IPortfolioService
{
    Task<List<Domain.Entities.Project>> GetProjectsAsync(CancellationToken ct = default);
    Task<List<Domain.Entities.Experience>> GetExperiencesAsync(CancellationToken ct = default);
    Task<List<Domain.Entities.Skill>> GetSkillsAsync(CancellationToken ct = default);
    Task<List<Domain.Entities.Education>> GetEducationsAsync(CancellationToken ct = default);
    Task<List<Domain.Entities.Certification>> GetCertificationsAsync(CancellationToken ct = default);
    Task<PortfolioData> GetAllDataAsync(CancellationToken ct = default);
}

public record PersonalInfo(
    string FirstName,
    string LastName,
    string Title,
    string Description,
    string GithubUrl,
    string LinkedinUrl,
    string Email
);

public record PortfolioData(
    PersonalInfo Profile,
    List<Domain.Entities.Project> Projects,
    List<Domain.Entities.Experience> Experiences,
    List<Domain.Entities.Skill> Skills,
    List<Domain.Entities.Education> Educations,
    List<Domain.Entities.Certification> Certifications
);