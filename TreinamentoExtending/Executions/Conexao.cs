using System;
using System.Net;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;

namespace DrownSolutions
{
    class Conexao
    {

        private static CrmServiceClient crmServiceClientDestino;
        private static CrmServiceClient crmServiceClientOrigem;

        public CrmServiceClient ObterOrigem()
        {
            //--------------Ambiente para Praticar -----------------//

            var connectionStringCRM = @"AuthType=OAuth;
            Username = flavio@academiacrm2021.onmicrosoft.com;
            Password = R3s1d3nt2; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org8904b810.crm2.dynamics.com/main.aspx;";

            if (crmServiceClientDestino == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                crmServiceClientDestino = new CrmServiceClient(connectionStringCRM);
            }
            return crmServiceClientDestino;
        }
        
        public CrmServiceClient ObterDestino()
        {
            //-------Ambiente acompanhado do Professor ------------//

            var connectionStringCRM = @"AuthType=OAuth;
            Username = flavio@drownsolutions.onmicrosoft.com;
            Password = R3s1d3nt2; SkipDiscovery = True;
            AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
            RedirectUri = app://58145B91-0C36-4500-8554-080854F2AC97;
            Url = https://org45e40928.crm2.dynamics.com/main.aspx;";

            if (crmServiceClientOrigem == null)
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                crmServiceClientOrigem = new CrmServiceClient(connectionStringCRM);
            }
            return crmServiceClientOrigem;
        }
    }
}
