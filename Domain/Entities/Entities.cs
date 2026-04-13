namespace Portfolio.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string FullDescription { get; set; } = string.Empty;
        public string Challenges { get; set; } = string.Empty;
        public string Stack { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? Icon { get; set; }
        public string? DemoUrl { get; set; }
        public string? GithubUrl { get; set; }
        public string Handling { get; set; } = string.Empty; // How it was handled/managed
        public string RecruiterPerspective { get; set; } = string.Empty; // Why a recruiter would love this
    }

    public class Experience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // Backend/Frontend/Tools
        public int Proficiency { get; set; } // 1-100
    }

    public class Education
    {
        public int Id { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string Period { get; set; } = string.Empty;
        public string? Details { get; set; }
    }

    public class Certification
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
    }

    public class PersonalInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string GithubUrl { get; set; } = string.Empty;
        public string LinkedinUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
