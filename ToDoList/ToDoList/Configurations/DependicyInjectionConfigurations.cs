using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Bll.Dto;
using ToDoList.Bll.ProFile;
using ToDoList.Bll.Services;
using ToDoList.Bll.Validators;
using ToDoList.Repository.Services;

namespace ToDoList.Server.Configurations;

public static class DependicyInjectionConfigurations
{
    public static void Configure(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IToDoItemRepository, AdoNetToDoItemRepository>();
        builder.Services.AddScoped<IToDoItemService, ToDoItemService>();
        builder.Services.AddAutoMapper(typeof(MappingProFile));

        builder.Services.AddScoped<IValidator<ToDoItemCreateDto>, ToDoItemCreateDtoValidator>();
        builder.Services.AddScoped<IValidator<ToDoItemUpdateDto>, ToDoItemUpdateDtoValidator>();
    }
}
