$('.updateBtn').on('click', function (event) {
    event.preventDefault();

    if ($('#updateUserForm').valid()) {
        // Show loading dialog
        const loadingDialog = Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showConfirmButton: false, // Hide OK button
            willOpen: () => {
                Swal.showLoading();
            },
        });

        var data = $("#updateUserForm").serialize();

        $.ajax({
            type: 'POST',
            url: '/User/Update/' + $("#UserId").val(),
            contentType: 'application/x-www-form-urlencoded',
            data: data,
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
                            window.location.href = '/User/Index';
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
                            window.location.href = '/User/Index';
                        }
                    });
                }
            },
            error: function (error) {

            }
        });
    }
    else {
        return false;
    }
});