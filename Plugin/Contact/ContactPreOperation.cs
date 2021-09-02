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
    public class ContactPreOperation : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.MessageName.ToLower() == "create" && context.Mode == Convert.ToInt32(MeuEnum.Mode.Synchronous) &&
                context.Stage == Convert.ToInt32(MeuEnum.Stage.PreOperation))
            {

                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var serviceUser = serviceFactory.CreateOrganizationService(context.UserId);
                var trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

                trace.Trace("Inicio Plugin");
                Entity entidadeContexto = null;

                if (context.InputParameters.Contains("Target"))
                    entidadeContexto = (Entity)context.InputParameters["Target"];

                if (entidadeContexto.LogicalName == "contact")
                {
                    var fetch = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                          <entity name='contact'>
                            <attribute name='fullname' />
                            <attribute name='telephone1' />
                            <attribute name='contactid' />
                            <order attribute='fullname' descending='false' />
                            <filter type='and'>
                               <condition attribute='ds_cpf' operator='eq' value='{entidadeContexto["ds_cpf"]}' />
                              </filter>
                           </entity>
                         </fetch>";

                    var Retorno = serviceUser.RetrieveMultiple(new FetchExpression(fetch));

                    if (Retorno.Entities.Count > 0)
                    {
                        throw new InvalidPluginExecutionException("Cpf já existente!!");
                    }
                }
            }
        }
    }
}

