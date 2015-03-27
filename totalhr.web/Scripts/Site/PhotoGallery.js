var photoid = 0;

function DisplayLargePicture(objPicture) {
    
    var url = objPicture.attr('data-large');
    photoid = objPicture.attr('data-photoid');

    $('#modalImg').attr('src', url);

    var options = {
        "backdrop": "static"
    }

    $('#basicModal').modal(options);
}

function ConfirmDelete() {
    var choice = confirm($('#hdnConfirmDeleteMessage').val());

    if (choice) {
        DeletePhoto(photoid);
    }

    return;
}

function DeletePhoto(pid) {
   
    $.ajax({
        url: "/File/DeleteGalleryPhoto/" + pid,
        type: "POST",
        success: function (data, textStatus, jqXHR) {
            alert(data.Message);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
   
}