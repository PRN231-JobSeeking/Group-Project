using ClientRepository.Service;
using ClientRepository.Service.Implementation;
using System.Linq.Expressions;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Auth/Login", "");
}); ;
//builder.Services.AddHttpClient();

builder.Services
    .AddHttpClient("BaseClient", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration.GetSection("ApiURI").Value);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });
builder.Services.AddSession();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ISkillService, SkillService>();
builder.Services.AddTransient<IAuthenService, AuthenService>();
builder.Services.AddTransient<IUserSkillService, UserSkillService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();
builder.Services.AddTransient<ISlotService, SlotService>();
builder.Services.AddTransient<IInterviewService, InterviewService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ILocationService, LocationService>();
builder.Services.AddTransient<ILevelService, LevelService>();
builder.Services.AddTransient<IFeedbackService, FeedbackService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();


app.MapRazorPages();

app.Run();
