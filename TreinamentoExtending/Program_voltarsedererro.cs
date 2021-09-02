using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Net;
using Microsoft.Xrm.Tooling.Connector;

namespace DrownSolutions
{
    class Program
    {
        static void Main(string[] args)
        {
            //conexao para qualquer ambiente para coletar dados
            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();

            //conexao ambiente do tcc sera essa abaixo
            Conexao conexaoDestino = new Conexao();
            var serviceproxyDestino = conexaoDestino.ObterDestino();

            //chamar metodos para execucao

            //ImportarConta(serviceproxyDestino);
            ImportarContato(serviceproxyDestino);
            //ImportarClientePotencial(serviceproxyDestino);
            //ImportarPedidos(serviceproxyOrigem);
            //ImportarListadePreco(serviceproxyDestino);
            //ImportarItensPedidos(serviceproxyOrigem);

            Console.WriteLine("FIM DA IMPORTAÇÃO!");
            Console.ReadKey();

        }

        #region TCC BOT

        #region Importar Conta
        //metodo para importar contas de outro CRM
        static void ImportarConta(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("account");
            queryExpression.Criteria.AddCondition("name", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("name", "telephone1");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("account");
                Guid registro = new Guid();

                if (item.Attributes.Contains("name"))
                    entidade.Attributes.Add("name", item["name"]);
                entidade.Attributes.Add("telephone1", item["telephone1"]);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region Importar Contato
        //metodo para importar contatos de outro CRM
        static void ImportarContato(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("contact");
            queryExpression.Criteria.AddCondition("firstname", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("lastname", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("firstname", "lastname");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("contact");
                Guid registro = new Guid();

                if (item.Attributes.Contains("firstname"))
                    entidade.Attributes.Add("firstname", item["firstname"]);
                entidade.Attributes.Add("lastname", item["lastname"]);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region PriceLevel
        static void ImportarListadePreco(CrmServiceClient serviceproxyOrigem)
        {
            QueryExpression queryExpression = new QueryExpression("pricelevel");
            queryExpression.Criteria.AddCondition("pricelevelid", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("name", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("msdyn_module", ConditionOperator.NotNull);

            queryExpression.ColumnSet = new ColumnSet("pricelevelid", "name", "msdyn_module");

            Conexao conexaoDestino = new Conexao();
            var serviceproxyDestino = conexaoDestino.ObterDestino();
            EntityCollection colecaoEntidadesDestino = serviceproxyDestino.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesDestino.Entities)
            {
                var entidade = new Entity("pricelevel");
                Guid registro = item.Id;

                if (item.Attributes.Contains("pricelevelid"))
                    entidade.Attributes.Add("pricelevelid", item["pricelevelid"]);
                    entidade.Attributes.Add("name", item["name"]);
                    entidade.Attributes.Add("msdyn_module", item["msdyn_module"]);

                registro = serviceproxyOrigem.Create(entidade);

            }

        }


        #endregion

        #region Importar Lead
        //metodo para importar contatos de outro CRM
        static void ImportarClientePotencial(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("lead");
            queryExpression.Criteria.AddCondition("subject", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("lastname", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("subject", "firstname", "lastname");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("lead");
                Guid registro = new Guid();

                if (item.Attributes.Contains("subject"))
                    entidade.Attributes.Add("subject", item["subject"]);
                entidade.Attributes.Add("firstname", item["firstname"]);
                entidade.Attributes.Add("lastname", item["lastname"]);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region Importar SalesOrderDetails
        /*static object ImportarItensPedidos(CrmServiceClient serviceproxyOrigem)
        {

            QueryExpression queryExpression = new QueryExpression("salesorderdetail");
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            queryExpression.Criteria.AddCondition("salesorderid", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("salesorderdetailname", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("productdescription", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("priceperunit", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("quantity", ConditionOperator.NotNull);
           
            Conexao conexaoDestino = new Conexao();
            var serviceproxyDestino = conexaoDestino.ObterDestino();
            EntityCollection colecaoEntidadesDestino = serviceproxyDestino.RetrieveMultiple(queryExpression);

            var entidade = new Entity("salesorderdetail");
            Guid registro = new Guid();

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {

                Guid Value = (Guid)item.Attributes["salesorderid"];
                EntityReference pedidoId = new EntityReference("salesorder", Value);
                
                if (item.Attributes.Contains("salesorderid"))
                    entidade.Attributes.Add("salesorderid", item["pedidoId"]);

                if (item.Attributes.Contains("salesorderdetailname"))
                    entidade.Attributes.Add("salesorderdetailname", item["salesorderdetailname"]);

                if (item.Attributes.Contains("productdescription"))
                    entidade.Attributes.Add("productdescription", item["productdescription"]);

                if (item.Attributes.Contains("priceperunit"))
                    entidade.Attributes.Add("priceperunit", item["priceperunit"]);

                if (item.Attributes.Contains("quantity"))
                    entidade.Attributes.Add("quantity", item["quantity"]);

                registro = serviceproxyOrigem.Create(entidade);

            }

        }*/

        #endregion

        #region Importar SalesOrder
        static void ImportarPedidos(CrmServiceClient serviceproxyOrigem)
        {
            QueryExpression queryExpression = new QueryExpression("salesorder");
            queryExpression.Criteria.AddCondition("ordernumber", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("name", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("pricelevelid", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("ispricelocked", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("customerid", ConditionOperator.NotNull);

            queryExpression.ColumnSet = new ColumnSet("ordernumber", "name", "ispricelocked", "customerid");

            Conexao conexaoDestino = new Conexao();
            var serviceproxyDestino = conexaoDestino.ObterDestino();
            EntityCollection colecaoEntidadesDestino = serviceproxyDestino.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesDestino.Entities)
            {
                var entidade = new Entity("salesorder");
                Guid registro = item.Id;

                if (item.Attributes.Contains("ordernumber"))
                    entidade.Attributes.Add("ordernumber", item["ordernumber"]);

                if (item.Attributes.Contains("name"))
                    entidade.Attributes.Add("name", item["name"]);

               if (item.Attributes.Contains("pricelevelid"))
                   entidade.Attributes.Add("pricelevelid", ((EntityReference)item["pricelevelid"]));

                if (item.Attributes.Contains("ispricelocked"))
                    entidade.Attributes.Add("ispricelocked", item["ispricelocked"]);

                if (item.Attributes.Contains("customerid"))
                    entidade.Attributes.Add("customerid", ((EntityReference)item["customerid"]));

                registro = serviceproxyOrigem.Create(entidade);

            }

        }
        #endregion

        #endregion BOT TCC

    }
}