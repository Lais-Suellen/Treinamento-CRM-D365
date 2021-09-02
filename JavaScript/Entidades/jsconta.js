function OnChange(executionContext) { // Evento Onchange
    debugger;

    var formContext = executionContext.getFormContext();
    var cnpj = formContext.getAttribute("ds_cnpj").getValue();
    var cep = formContext.getAttribute("address1_postalcode").getValue();

    /*--------------------------------VALIDAR CNPJ---------------------------------------*/
    if (cnpj != null) { // se o CNPJ não for igual a nulo retorna verdadeiro

        function validarCNPJ(cnpj) {  // Aqui dentro da função VALIDAR CNPJ
            cnpj = cnpj.replace(/[^\d]+/g, ''); // ELIMINAMOS TODOS OS CARAC NÃI NUMERICOS PASSADOS COMO PARAMENTRO.


            if (cnpj == '') return false; // SE O CNPJ FOR IGUAL A 0 RETORNA FALSO

            if (cnpj.length != 14) // VERIFICA SE A QUANTIDADE DE NUMERAIS É diferente A 14
                return false;

            // Elimina CNPJs invalidos conhecidos // Temos a string e checamos se o formado digitado por valores iguais
            if (cnpj == "00000000000000" || cnpj == "11111111111111" || cnpj == "22222222222222" || cnpj == "33333333333333" ||
                cnpj == "44444444444444" || cnpj == "55555555555555" || cnpj == "66666666666666" || cnpj == "77777777777777" ||
                cnpj == "88888888888888" ||
                cnpj == "99999999999999")
                return false;

            // Valida DVs // verificamos se os dois digitos verificadores  digitados é valido em caso negativo retorna falso.
            tamanho = cnpj.length - 2
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;

            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;

            return true;;; //retorna verdadeiro 

        }

        if (validarCNPJ(cnpj) == false) {
            Xrm.Navigation.openAlertDialog("CNPJ Inválido");
        }
    }
    /*---------------------------------Mascara para o CNPJ--------------------------------*/

    if (cnpj != null) {
        cnpj = cnpj.replace(/\D/g, "");

        //Mascara
        var comeco = cnpj.substring(0, 2);
        var meio = cnpj.substring(2, 5);
        var fim = cnpj.substring(5, 8);
        var afterBar = cnpj.substring(8, 12);
        var digitos = cnpj.substring(12, 14);
        cnpj = comeco + "." + meio + "." + fim + "/" + afterBar + "-" + digitos;
        formContext.getAttribute("ds_cnpj").setValue(cnpj);
    }
    //--------------------------------- Verificar se o CNPJ já está no banco de dados no cadastro --------------------------------
    if (Xrm.Page.ui.getFormType() == 1) {
        debugger;
        Xrm.WebApi.retrieveMultipleRecords("account", "?$select=ds_cnpj&$filter=contains(ds_cnpj, '" + cnpj + "')").then(
            function success(result) {
                if (result.entities.length > 0) {
                    Xrm.Navigation.openAlertDialog('CNPJ Ja existe no cadastro');

                }
            },
            function (error) {
                console.log(error.message);

            }
        );
    }
    //--------------------------------- Verificar se o CNPJ já está no banco de dados na atualizacao --------------------------------
    if (Xrm.Page.ui.getFormType() == 2) {
        debugger;
        Xrm.WebApi.retrieveMultipleRecords("account", "?$select=ds_cnpj&$filter=contains(ds_cnpj, '" + cnpj + "')").then(
            function success(result) {
                if (result.entities.length > 0) {
                    Xrm.Navigation.openAlertDialog('CNPJ Ja existe no cadastro');

                }
            },
            function (error) {
                console.log(error.message);

            }
        );
    }

 //////////////////////////////////////////---------CEP---------////////////////////////////////////////////////////

    /*------------------------------Mascara para o CEP-------------------------------------------*/

    if (cep != null) { // verifica se o campo está vazio para evitar erro ao aplicar a máscara

        cep = cep.replace(/\D/g, ""); //aceita apenas a digitação de números no campo.
        var comeco = cep.substring(0, 5);
        var fim = cep.substring(5, 8);
        cep = comeco + "-" + fim;
        formContext.getAttribute("address1_postalcode").setValue(cep);
    }
} //FINAL DO EVENTO ONCHANGE