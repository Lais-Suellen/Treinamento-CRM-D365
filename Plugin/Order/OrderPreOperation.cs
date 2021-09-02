using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PluginDrownSolutions;

namespace Plugin
{
    public class OrderPreOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName.ToLower() == "create" && context.Mode == Convert.ToInt32(MeuEnum.Mode.Synchronous) && // pre operation antes de eu validar a informaçao, sicrono
                context.Stage == Convert.ToInt32(MeuEnum.Stage.PreOperation))
            {

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var serviceUser = serviceFactory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                trace.Trace("Inicio Plugin"); //  log do banco
                Entity entidadeContexto = null;

                if (context.InputParameters.Contains("Target")) // target é um metodo
                    entidadeContexto = (Entity)context.InputParameters["Target"];

                if (entidadeContexto.LogicalName == "cr81f_saleorder")
                {
                    var fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                                  <entity name='cr81f_saleorder'>
                                <attribute name='cr81f_saleorder'/>
                                <filter type='and'>
                            <condition attribute='cr81f_saleorder' operator='eq' value='{entidadeContexto["cr81f_saleorder"]}'/>
                              </filter>
                            </entity>
                        </fetch>"; // usamos fetch pq já está estruturado para CRM

                    var Retorno = serviceUser.RetrieveMultiple(new FetchExpression(fetch));

                    if (Retorno.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("Pedido ja existente!!");
                    }
                }
            }
        }
    }
}

