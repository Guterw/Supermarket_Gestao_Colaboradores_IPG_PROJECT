// subordinados.js
$(document).ready(function () {
    $('#SuperiorId').change(function () {
        var funcionarioId = $(this).val();

        $.ajax({
            type: 'POST',
            url: '/Home/ListarSubordinados',
            data: { id: funcionarioId },
            success: function (result) {
                $('#listaSubordinados').html(result);

                // Mostrar a div de subordinados
                $('#divSubordinados').show();
            },
            error: function () {
                console.error('Erro ao obter subordinados.');
            }
        });
    });

    // Evento do botão Voltar
    $('#btnVoltar').click(function () {
        // Esconder a div de subordinados
        $('#divSubordinados').hide();
    });
});
