$(document).ready(function () {

    // 1. MÁSCARA DE TELEFONE (Híbrida: 8 ou 9 dígitos) *PEGUEI NA INTERNETTTT*
    var behavior = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        options = {
            onKeyPress: function (val, e, field, options) {
                field.mask(behavior.apply({}, arguments), options);
            }
        };
    $('#campoTelefone').mask(behavior, options);
  
    // LÓGICA DE CPF / CNPJ (SEPARADOS PELO DROPDOWN)
  
    function atualizarMascara() {
        var tipo = $('#NovoCliente_TipoPessoa option:selected').text();
        var campo = $('#campoCpf');

        // Remove máscara antiga para evitar conflito
        campo.unmask();

        if (tipo.indexOf('Jurídica') >= 0 || tipo === 'PJ') {
            // MÁSCARA CNPJ
            campo.mask('00.000.000/0000-00', { reverse: true });
            campo.attr('placeholder', '00.000.000/0000-00');
        } else {
            // MÁSCARA CPF
            campo.mask('000.000.000-00', { reverse: true });
            campo.attr('placeholder', '000.000.000-00');
        }
    }

    // Atualiza quando muda o dropdown
    $('#NovoCliente_TipoPessoa').change(function () {
        $('#campoCpf').val(''); // Limpa o campo
        atualizarMascara();
    });

    // Inicializa
    atualizarMascara();

     //COLAR (PASTE) COM FORMATAÇÃO VISUAL
    $('#campoCpf').on('paste', function (e) {
        e.preventDefault(); // Cancela o colar nativo do navegador

        var tipo = $('#NovoCliente_TipoPessoa option:selected').text();
        var isJuridica = (tipo.indexOf('Jurídica') >= 0 || tipo === 'PJ');

        //pega os números colados e limpa
        var textoCopiado = (e.originalEvent || e).clipboardData.getData('text/plain');
        var apenasNumeros = textoCopiado.replace(/\D/g, '');

        // 2. Define o valor cortado (11 digitos se CPF, 14 se CNPJ)
        var valorFinal;
        if (isJuridica) {
            valorFinal = apenasNumeros.substring(0, 14);
        } else {
            valorFinal = apenasNumeros.substring(0, 11);
        }

        // 3. Aplica o valor e FORÇA A FORMATAÇÃO (trigger input)
        $(this).val(valorFinal);
        $(this).trigger('input'); 
    });

    // ============================================================

    // 3. MÁSCARA DE CEP 
    $('#campoCep').mask('00000-000');

    // 4. PESQUISA NA TABELA ***PEGUEI NA INTERNET***
    $("#inputPesquisa").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("table tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

});

// FUNÇÃO DE EXCLUSÃO
function confirmarExclusao(event, form) {
    event.preventDefault();
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

// Faz o Enter focar no botão salvar
$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 13) {
            if ($("#modalCadastro").is(":visible")) {
            }
        }
    });
});

$('.btn-close, .btn-secondary').on('click', function () {
    // 1. Zera os textos ao clicar no Xzinho
    $('#modalCadastro form').trigger('reset');

    // 2. Volta o dropdown pro padrão e ajusta a máscara
    $('#NovoCliente_TipoPessoa').val('Pessoa Física').change();
});