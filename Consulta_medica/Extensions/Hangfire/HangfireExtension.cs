using Consulta_medica.Application.Interfaces;
using Consulta_medica.Static;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Consulta_medica.Extensions.Hangfire
{
    public static class HangfireExtension
    {
        public static IServiceProvider HangfireExecuteJob(this IServiceProvider serviceProvider, IRecurringJobManager recurringJobManager) 
        {
            var CitasService = serviceProvider.GetService<ICitasMedicasService>();

            recurringJobManager.AddOrUpdate(JobName.NOTI_RECORDATORIO_CITA, () => CitasService.RecordatorioNotification(), Cron.Daily(6+5));

            return serviceProvider;
        }
    }
}
