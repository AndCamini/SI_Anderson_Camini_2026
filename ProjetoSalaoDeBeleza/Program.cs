using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetoSalaoDeBeleza.Components;
using ProjetoSalaoDeBeleza.Components.Account;
using ProjetoSalaoDeBeleza.Data;
using ProjetoSalaoDeBeleza.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents(options =>
    options.DetailedErrors = true)
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<PaisesService>();
builder.Services.AddScoped<EstadosService>();
builder.Services.AddScoped<CidadesService>();
builder.Services.AddScoped<ClientesService>();
builder.Services.AddScoped<FuncionariosService>();
builder.Services.AddScoped<CondicaoPagamentoService>();
builder.Services.AddScoped<CategoriasService>();
builder.Services.AddScoped<ProdutosService>();
builder.Services.AddScoped<FornecedoresService>();
builder.Services.AddScoped<TransportadoresService>();
builder.Services.AddScoped<VeiculosService>();
builder.Services.AddScoped<MarcasVeiculosService>();
builder.Services.AddScoped<TiposVeiculoService>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
