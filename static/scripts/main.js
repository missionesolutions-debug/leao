$(document).ready(function () {
    if ($('.alert').length > 0) {
        $('.alert').delay(5000).slideUp();
    };

    if ($('.chosen').length > 0) {
        $('.chosen').chosen({
            no_results_text: "Oops, não encontramos nada!"
        })
    };

    //if ($('.summernote-minimal').length > 0) {
    //    $('.summernote-minimal').summernote({
    //        placeholder: 'Hello stand alone ui',
    //        tabsize: 2,
    //        height: 120,
    //        toolbar: [
    //            ['style', ['style']],
    //            ['font', ['bold', 'underline', 'clear']],
    //            ['para', ['ul', 'ol', 'paragraph']],
    //            ['table', ['table']],
    //            ['view', ['fullscreen']]
    //        ]
    //    });
    //}

    //PROGRESS-BAR
    $('#Nome').on('input', function (e) {
        if ($('#Url').length > 0) {

            //recupera valor
            var valor = $(this).val();

            $('#PageTitle').val(valor);

            //remove acentos
            valor = valor.normalize('NFD').replace(/[\u0300-\u036f]/g, "")

            //remove espaços e coloca em lowercase
            valor = valor.replace(/[^a-z0-9\s]/gi, '-').replace(/[_\s]/g, '-').split(' ').join('-').toLowerCase();

            $('#Url').val(valor);

            var domain = $('.google-snippet span').data('domain');
            var route = $('.google-snippet span').data('route');
            $('.google-snippet span').html(domain + route + valor);
        }
    });

    $('#Titulo').on('input', function (e) {
        if ($('#Url').length > 0) {

            //recupera valor
            var valor = $(this).val();

            $('#PageTitle').val(valor);

            //remove acentos
            valor = valor.normalize('NFD').replace(/[\u0300-\u036f]/g, "")

            //remove espaços e coloca em lowercase
            valor = valor.replace(/[^a-z0-9\s]/gi, '-').replace(/[_\s]/g, '-').split(' ').join('-').toLowerCase();

            $('#Url').val(valor);
           

            var domain = $('.google-snippet span').data('domain');
            var route = $('.google-snippet span').data('route');
            $('.google-snippet span').html(domain + route + valor);
        }
    });

    $('#PageTitle').on('input', function (e) {
        //recupera valor
        var valor = $(this).val();

        $('.google-snippet a').html(valor);
    });

    $('#Url').on('input', function (e) {
        //recupera valor
        var valor = $(this).val();

        var domain = $('.google-snippet span').data('domain');
        var route = $('.google-snippet span').data('route');

        valor = valor.normalize('NFD').replace(/[\u0300-\u036f]/g, "")

        //remove espaços e coloca em lowercase
        valor = valor.replace(/[^a-z0-9\s]/gi, '-').replace(/[_\s]/g, '-').split(' ').join('-').toLowerCase();

        $('.google-snippet span').html(domain + route + valor);
    });

    $('#MetaDescription').on('input', function (e) {
        //recupera valor
        var valor = $(this).val();

        $('.google-snippet p').html(valor);
    });
   
});

function callSetJobTaskId(id) {
    $('#jobtaskId').val(id);
}

function callDelete(button, id) {
    //var data = $(button).data('href');

    $.alert({
        icon: 'fa fa-warning',
        title: 'Deletar',
        content: 'Deseja mesmo deletar?',
        type: 'green',
        buttons: {
            confirm: {
                text: 'Deletar',
                btnClass: 'btn-green',
                action: function () {
                    $("#form-"+id).submit()
                }
            },
            cancel: {
                text: 'Voltar',
            },
        }
    });

}