let table = new DataTable('#townshipGrid');
function deleteTownship(id) {
    //let id = $(this).data("id");
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
                url: '/Township/Delete/' + id,
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
                                window.location.href = '/Township/Index';
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
                                window.location.href = '/Township/Index';
                            }
                        });
                    }
                },
                error: function (error) {

                }
            });
        }
    });
}