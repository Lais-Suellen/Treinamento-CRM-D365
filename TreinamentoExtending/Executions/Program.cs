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
            //ImportarContato(serviceproxyDestino);
            //ImportarClientePotencial(serviceproxyDestino);
            //ImportarPedidos(serviceproxyDestino);
            //ImportarItensPedidos(serviceproxyDestino);

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
            queryExpression.ColumnSet = new ColumnSet("name", "telephone1","fax","tt_cnpj");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("account");
                Guid registro = item.Id;

                if (item.Attributes.Contains("accountid"))
                    entidade.Attributes.Add("accountid", registro);
                    entidade.Attributes.Add("name", item["name"]);
                    entidade.Attributes.Add("telephone1", item["telephone1"]);
                    entidade.Attributes.Add("fax", item["fax"]);
                    entidade.Attributes.Add("ds_cnpj", item["tt_cnpj"]);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region Importar Contato
        //metodo para importar contatos de outro CRM
        static void ImportarContato(CrmServiceClient serviceproxyDestino) //Criamos o metodo Importar Contato faz a conexão com CRM LEGADO
        {
            QueryExpression queryExpression = new QueryExpression("contact");  // consultamos no tabela contato 
            queryExpression.Criteria.AddCondition("firstname", ConditionOperator.NotNull); 
            queryExpression.Criteria.AddCondition("lastname", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("firstname", "lastname", "tt_cpf"); ;// PREPARAMOS  BUSCA E INFORMAÇÕES QUE QUEREMOS

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem(); // CONEXÃO COM O NOVO SISTEMA
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities) // temos o forech onde executamos a instrução desse bloco, CAPTAMOS A INF DO SISTEMA LEGADO PARA GRAVAMOS AS INF NO SISTEMA DE DESTINO
                                                                  // 
            {
                var entidade = new Entity("contact"); 
                Guid registro = item.Id; // banco de dados
                                            
                if (item.Attributes.Contains("contactid"))
                    entidade.Attributes.Add("contactid", registro);
                    entidade.Attributes.Add("firstname", item["firstname"]);
                    entidade.Attributes.Add("lastname", item["lastname"]);
                    entidade.Attributes.Add("ds_cpf", item["tt_cpf"]);

                registro = serviceproxyDestino.Create(entidade); // DEPOIS DE CONSEGUIR  BUSCAR / PUXAR AS INFORMAÇÕES PARA O SISTEMA DO CRM DESTINO

            }

        }
        #endregion

        #region Importar Lead
        //metodo para importar contatos de outro CRM
        static void ImportarClientePotencial(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("tt_clientepotencial");
            queryExpression.Criteria.AddCondition("tt_assunto", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_sobrenome", ConditionOperator.NotNull);
            queryExpression.ColumnSet = new ColumnSet("tt_assunto", "tt_clientepotencial", "tt_sobrenome", "tt_softwareparatestes"
                ,"tt_email","tt_telefonecomercial","tt_empresa");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("lead");
                Guid registro = item.Id;

                if (item.Attributes.Contains("tt_clientepotencialid"))
                    entidade.Attributes.Add("leadid", registro);
                    entidade.Attributes.Add("subject", item["tt_assunto"]);
                    entidade.Attributes.Add("firstname", item["tt_clientepotencial"]);
                    entidade.Attributes.Add("lastname", item["tt_sobrenome"]);
                    entidade.Attributes.Add("emailaddress1", item["tt_email"]);
                    entidade.Attributes.Add("telephone1", item["tt_telefonecomercial"]);
                    entidade.Attributes.Add("companyname", item["tt_empresa"]);
                    entidade.Attributes.Add("ds_produtoorcamento", item["tt_softwareparatestes"]);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region SalesOrder
        static void ImportarPedidos(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("tt_pedido");
            queryExpression.Criteria.AddCondition("tt_numeropedido", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_nome", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_conta", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_contato", ConditionOperator.NotNull);

            queryExpression.ColumnSet = new ColumnSet("tt_conta", "tt_contato", "tt_descricao", "tt_enderecoemail",
                "tt_nome", "tt_numeropedido", "tt_pedidoid");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("ds_pedido");
                Guid registro = item.Id;

                if (item.Attributes.Contains("tt_numeropedido"))
                    entidade.Attributes.Add("ds_pedido", item["tt_numeropedido"]);

                if (item.Attributes.Contains("tt_conta"))
                    entidade.Attributes.Add("ds_conta", item["tt_conta"]);

                if (item.Attributes.Contains("tt_contato"))
                    entidade.Attributes.Add("ds_contato", item["tt_contato"]);

                if (item.Attributes.Contains("tt_descricao"))
                    entidade.Attributes.Add("ds_descricao", item["tt_descricao"]);

                if (item.Attributes.Contains("tt_enderecoemail"))
                    entidade.Attributes.Add("ds_email", item["tt_enderecoemail"]);

                if (item.Attributes.Contains("tt_nome"))
                    entidade.Attributes.Add("ds_nome", item["tt_nome"]);

                if (item.Attributes.Contains("tt_pedidoid"))
                    entidade.Attributes.Add("ds_pedidoid", registro);

                registro = serviceproxyDestino.Create(entidade);

            }

        }
        #endregion

        #region SalesOrderDetails

        static void ImportarItensPedidos(CrmServiceClient serviceproxyDestino)
        {
            QueryExpression queryExpression = new QueryExpression("tt_itenspedido");
            queryExpression.Criteria.AddCondition("tt_itenspedido", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_descricao", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_valor", ConditionOperator.NotNull);
            queryExpression.Criteria.AddCondition("tt_idpedido", ConditionOperator.NotNull);

            queryExpression.ColumnSet = new ColumnSet("tt_descricao", "tt_idpedido", "tt_nomedoproduto", "tt_quantidade", "tt_valor", "tt_itenspedido", "tt_itenspedidoid");

            Conexao conexaoOrigem = new Conexao();
            var serviceproxyOrigem = conexaoOrigem.ObterOrigem();
            EntityCollection colecaoEntidadesOrigem = serviceproxyOrigem.RetrieveMultiple(queryExpression);

            foreach (var item in colecaoEntidadesOrigem.Entities)
            {
                var entidade = new Entity("ds_itensdepedidos");
                Guid registro = item.Id;

                if (item.Attributes.Contains("tt_itenspedidoid"))
                    entidade.Attributes.Add("ds_itensdepedidosid", registro);

                if (item.Attributes.Contains("tt_itenspedido"))
                    entidade.Attributes.Add("ds_itenspedidos", item["tt_itenspedido"]);

                if (item.Attributes.Contains("tt_idpedido"))
                {
                    EntityReference buscapedido = item.GetAttributeValue<EntityReference>("tt_idpedido");
                    string idpedido = buscapedido.Name;
                    QueryExpression queryExpressionds = new QueryExpression("ds_pedido");
                    queryExpressionds.Criteria.AddCondition("ds_pedido", ConditionOperator.Equal,idpedido);
                    EntityCollection colecaoEntidadesDestino = serviceproxyDestino.RetrieveMultiple(queryExpressionds);

                    if(colecaoEntidadesDestino.Entities.Count == 1)
                    {
                        Entity itenspedido = colecaoEntidadesDestino.Entities[0];
                        Guid itens = itenspedido.Id;
                        EntityReference numitens = new EntityReference("ds_itensdepedidos", itens);
                        entidade.Attributes.Add("cr81f_numpedido", numitens);
                    }
                 
                }

                if (item.Attributes.Contains("tt_nomedoproduto"))
                    entidade.Attributes.Add("ds_nomedoproduto", item["tt_nomedoproduto"]);

                if (item.Attributes.Contains("tt_descricao"))
                    entidade.Attributes.Add("ds_descricao", item["tt_descricao"]);

                if (item.Attributes.Contains("tt_valor"))
                    entidade.Attributes.Add("cr81f_valorprudot", item["tt_valor"]);

                if (item.Attributes.Contains("tt_quantidade"))
                    entidade.Attributes.Add("cr81f_quantidade", item["tt_quantidade"]);

                registro = serviceproxyDestino.Create(entidade);

            }
        }

        #endregion

        #endregion BOT TCC
    }
}
    