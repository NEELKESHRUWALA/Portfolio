using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Portfolio.Domain.Entities;
using Svc = Portfolio.Application.Services.IPortfolioService;
using Portfolio.Application.Services;

namespace Portfolio.Components.Pages;

public partial class Home
{
    [Inject] private Svc PortfolioSvc { get; set; } = null!;
    
    private PortfolioData? _data;
    private bool _isLoading = true;
    private Project selectedProject = new();
    private bool showModal;

    private List<Project> Projects => _data?.Projects ?? [];
    private List<Experience> Experiences => _data?.Experiences ?? [];
    private List<Skill> Skills => _data?.Skills ?? [];
    private List<Education> Educations => _data?.Educations ?? [];

    protected override async Task OnInitializedAsync()
    {
        _data = await PortfolioSvc.GetAllDataAsync();
        _isLoading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || _isLoading) return;
        await JSRuntime.InvokeVoidAsync("setupAnimations");
        await JSRuntime.InvokeVoidAsync("animateSkillBars");
    }

    private void SelectProject(Project p)
    {
        selectedProject = p;
        showModal = true;
    }
}
