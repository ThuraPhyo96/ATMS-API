let table = new DataTable('#atmGrid');

$('.btnDelete').on('click', function (event) {
    event.preventDefault();

    let id = $(this).data("id");
    Swal.fire({
        title: 'Confirmation?',
        text: "Are you sure that you want to delete?",
        icon: 'question',
        allowOutsideClick: false,
        showCancelButton: true,
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteATMLocation(id);
        }
    });
});

function deleteATMLocation(id) {
    // Show loading dialog
    const loadingDialog = Swal.fire({
        title: 'Loading...',
        allowOutsideClick: false,
        showConfirmButton: false, // Hide OK button
        willOpen: () => {
            Swal.showLoading();
        },
    });

    $.ajax({
        type: 'POST',
        url: '/ATMLocation/Delete/' + id,
        contentType: 'application/x-www-form-urlencoded',
        success: function (data) {
            loadingDialog.close();
            if (data.IsSuccess) {
                Swal.fire({
                    title: 'Success!',
                    text: data.Message,
                    icon: 'success',
                    allowOutsideClick: false,
                    showCancelButton: true,
                    confirmButtonText: 'OK',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/ATMLocation/Index';
                    }
                });
            }
            else {
                Swal.fire({
                    title: 'Error!',
                    text: data.Message,
                    icon: 'error',
                    allowOutsideClick: false,
                    showCancelButton: true,
                    confirmButtonText: 'OK',
                    cancelButtonText: 'Cancel'
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = '/ATMLocation/Index';
                    }
                });
            }
        },
        error: function (error) {

        }
    });
}