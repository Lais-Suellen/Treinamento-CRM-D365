using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactPreValidation
{
    public class AccountPreValidation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            //var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            //var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity entidadeContexto = null;

            if (context.InputParameters.Contains("Target"))
                entidadeContexto = (Entity)context.InputParameters["Target"];
            else
                return;

            if (!entidadeContexto.Contains("ds_cpf"))
                throw new InvalidPluginExecutionException("Cpf obrigatorio!!");
        }

    }

}