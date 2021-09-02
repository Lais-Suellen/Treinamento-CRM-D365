using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderPreValidation
{
    public class OrderPreValidation : IPlugin
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

            if (!entidadeContexto.Contains("cr81f_saleorder"))
                throw new InvalidPluginExecutionException("Numero do pedido obrigatorio!!");
        }

    }

}