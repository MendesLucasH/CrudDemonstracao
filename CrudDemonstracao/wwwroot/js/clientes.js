$(document).ready(function () {
    //1. MÁSCARA DE TELEFONE (Híbrida: 8 ou 9 dígitos) ***PEGUEI NA INTERNET***
    var behavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        options = {
            onKeyPress: function (val, e, field, options) {
                field.mask(behavior.apply({}, arguments), options);
            }
        };
    $('#campoTelefone').mask(behavior, options);
    // 2. MÁSCARA DE CPF OU CNPJ (Muda sozinha)     
    var cpfCnpjBehavior = function (val) {
        return val.replace(/\D/g, '').length <= 11 ? '000.000.000-009' : '00.000.000/0000-00';
    },
        cpfCnpjOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(cpfCnpjBehavior.apply({}, arguments), options);
            }
        };
    $('#campoCpf').mask(cpfCnpjBehavior, cpfCnpjOptions);

    // 3. MÁSCARA DE CEP 
    $('#campoCep').mask('00000-000');


   
});

function confirmarExclusao(event, form) {
    event.preventDefault(); // Trava o envio para o C# para esperar a resposta do usuário

    Swal.fire({
        title: 'Deseja excluir este cliente?',
        text: "Esta ação não poderá ser desfeita!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33', 
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sim, excluir!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            form.submit(); 
        }
    });
}