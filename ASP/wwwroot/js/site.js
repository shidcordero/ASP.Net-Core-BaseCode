// Showing message modal using Bootbox library
function showDefaultModal(message, title, size, confirmText = "Ok") {
    bootbox.alert({
        title: title,
        message: message,
        size: size,
        buttons: {
            ok: {
                label: confirmText,
                className: "btn-success"
            }
        }
    });
}

// Showing confirmation modal using Bootbox library
function showConfirmationModal(message, title = "Confirm", size = "medium", confirmText = "Yes", canceltext = "No") {
    const deffered = $.Deferred();

    bootbox.confirm({
        title: title,
        message: message,
        size: size,
        buttons: {
            confirm: {
                label: confirmText,
                className: "btn-success"
            },
            cancel: {
                label: canceltext,
                className: "btn-danger"
            }
        },
        callback: function (result) {
            if (result) {
                deffered.resolve(result);
            } else {
                deffered.reject(result);
            }
        }
    });

    return deffered.promise();
}