function OnChange(executionContext) {
    debugger;
    var formContext = executionContext.getFormContext();
    var cpf = formContext.getAttribute("ds_cpf").getValue();
    var cep = formContext.getAttribute("address1_postalcode").getValue();

    //////////////////////////////////////////---------CPF---------////////////////////////////////////////////////////

    /*--------------------------------VALIDAR CPF----------------------------------------*/
    if (cpf != null) {
        function validarCPF(cpf) {
            cpf = cpf.replace(/[^\d]+/g, '');

            if (cpf == '') return false;
            // Elimina CPFs invalidos conhecidos
            if (cpf.length != 11 ||
                cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" || cpf == "33333333333" ||
                cpf == "44444444444" || cpf == "55555555555" || cpf == "66666666666" || cpf == "77777777777" ||
                cpf == "88888888888" || cpf == "99999999999")
                return false;

            // Valida 1o digito	
            somador = 0;
            for (i = 0; i < 9; i++)
                somador += parseInt(cpf.charAt(i)) * (10 - i);
            resto = 11 - (somador % 11);
            if (resto == 10 || resto == 11)
                resto = 0;
            if (resto != parseInt(cpf.charAt(9)))
                return false;
            // Valida 2o digito	
            somador = 0;
            for (i = 0; i < 10; i++)
                somador += parseInt(cpf.charAt(i)) * (11 - i);
            resto = 11 - (somador % 11);
            if (resto == 10 || resto == 11)
                resto = 0;
            if (resto != parseInt(cpf.charAt(10)))
                return false;
            return true;
        }

        if (validarCPF(cpf) == false)
            Xrm.Navigation.openAlertDialog("CPF Inválido");
    }

    /*---------------------------------Mascara para o CPF--------------------------------*/

    if (cpf != null) { // verifica se o campo está vazio para evitar erro ao aplicar a máscara
        cpf = cpf.replace(/\D/g, "");

        var comeco = cpf.substring(0, 3);
        var meio = cpf.substring(3, 6);
        var fim = cpf.substring(6, 9);
        var digito = cpf.substring(9);
        cpf = comeco + "." + meio + "." + fim + "-" + digito;
        formContext.getAttribute("ds_cpf").setValue(cpf);
    }

    //--------------------------------- Verificar se o CPF já está no banco de dados no cadastro --------------------------------
    if (Xrm.Page.ui.getFormType() == 1) {
        debugger;
        Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=ds_cpf&$filter=contains(ds_cpf, '" + cpf + "')").then(
            function success(result) {
                if (result.entities.length > 0) {
                    Xrm.Navigation.openAlertDialog('CPF Ja existe no cadastro');

                }
            },
            function (error) {
                console.log(error.message);

            }
        );
    }
    //--------------------------------- Verificar se o CPF já está no banco de dados na atualizacao --------------------------------
    if (Xrm.Page.ui.getFormType() == 2) {
        debugger;
        Xrm.WebApi.retrieveMultipleRecords("contact", "?$select=ds_cpf&$filter=contains(ds_cpf, '" + cpf + "')").then(
            function success(result) {
                if (result.entities.length > 0) {
                    Xrm.Navigation.openAlertDialog('CPF Ja existe no cadastro');

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

}  //final do evento OnChange