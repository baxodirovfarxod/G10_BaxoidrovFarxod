using ToDoList.Repository.Settings;

namespace ToDoList.Server.Configurations;

public static class DatabaseConfigurations
{
    public static void Configuration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddSingleton(new SqlDbConnectionString(connectionString));
    }
}
