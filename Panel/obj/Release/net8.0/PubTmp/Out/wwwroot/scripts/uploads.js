$(document).ready(function () {
    $('.image-upload').change(function () {
        var dataID = $(this).data('id');
        var fileUpload = $(this).get(0);
        var files = fileUpload.files;
        var formData = new FormData();

        formData.append('file', files[0]);

        if (files.length > 0) {
            $.ajax({
                url: "/Imagem/FileUpload",
                type: "POST",
                contentType: false,
                processData: false,
                data: formData,
                // dataType: "json",
                success: function (result) {
                    $('.uploader-' + dataID).hide();
                    $('#' + dataID).val(result.fileName);
                    showImage(result.fileName, dataID);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    });

    $('.video-upload').change(function () {
        var dataID = $(this).data('id');
        var fileUpload = $(this).get(0);
        var files = fileUpload.files;
        var formData = new FormData();

        formData.append('file', files[0]);

        if (files.length > 0) {
            $.ajax({
                url: "/Video/FileUpload",
                type: "POST",
                contentType: false,
                processData: false,
                data: formData,
                // dataType: "json",
                success: function (result) {
                    $('.uploader-' + dataID).hide();
                    $('#' + dataID).val(result.fileName);
                    showVideo(result.fileName, dataID);
                },
                error: function (err) {
                    alert(err.statusText);
                }
            });
        }
    });

    $('.remover-img, .remover-musica, .remover-video').click(function () {
        hideImage($(this).data('id'));
    });
    $('.remover-img, .remover-musica').click(function () {
        hideImage($(this).data('id'));
    });
    $('.remover-video').click(function () {
        hideVideo($(this).data('id'));
    });

});
function hideVideo(id) {
    $('.bl-' + id).hide();
    $('.uploader-' + id).show();
    $('.bl-' + id + ' video source').attr('src', '');
    $('.bl-' + id + ' video')[0].load();
    $('#' + id).val("");

}

function hideImage(id) {
    $('.bl-' + id).hide();
    $('.uploader-' + id).show();
    $('.bl-' + id + ' img').attr('src', '');
    $('#' + id).val("");

}

function hideFile(id) {
    $('.bl-' + id).hide();
    $('.uploader-' + id).show();

    $('#' + id).val("");

}

function showFile(name, id) {
    $('.bl-' + id).show();
    $('.bl-' + id + ' a.download-file').attr("href", name);
}
function showImage(name, id) {
    $('.bl-' + id).show();
    $('.bl-' + id + ' img').attr('src', name);
}

function showVideo(name, id) {
    $('.bl-' + id).show();
    $('.bl-' + id + ' video source').attr('src', name);
    $('.bl-' + id + ' video')[0].load();
}
