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
    var aplicarMascara = function (valor) {
        var num = valor.replace(/\D/g, ''); 
        var campo = $('#campoCpf');

        campo.unmask(); 

        if (num.length > 11) {
            campo.mask('00.000.000/0000-00');
        } else {
            campo.mask('000.000.000-009');
        }
    };
    
    $('#campoCpf').on('input', function () {
        var val = $(this).val();
        
        if (val.length > 0) aplicarMascara(val);
    });

    $('#campoCpf').on('paste', function () {
        var elemento = $(this);        
        setTimeout(function () {
            var textoColado = elemento.val();
            aplicarMascara(textoColado);
        }, 150);
    });

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

$(document).ready(function () {
    $("#inputPesquisa").on("keyup", function () {
        var value = $(this).val().toLowerCase();

        $("table tbody tr").filter(function () {
            // Isso faz a busca olhar para o Nome e para o CPF/CNPJ ao mesmo tempo PEGUEI NA INTERNET**
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});