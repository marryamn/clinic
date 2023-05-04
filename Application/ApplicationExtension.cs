using System.Reflection;
using Application.AppointmentPay;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
          services.AddMediatR(Assembly.GetExecutingAssembly());

          services.AddTransient<IAppointmentPayService, AppointmentPayService>();
            return services;
        }
        
    }
}