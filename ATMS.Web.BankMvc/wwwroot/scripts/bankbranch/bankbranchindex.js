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
            deleteBankBranch(id);
        }
    });
});

function deleteBankBranch(id) {
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
        url: '/BankBranchAjax/Delete/' + id,
        contentType: 'application/x-www-form-urlencoded',
        success: function (data) {
            loadingDialog.close();
            if (data.IsSuccess) {
                window.location.href = '/BankBranchAjax/Index';
            }
            else {
                Swal.fire({
                    icon: "error",
                    title: "Oops...",
                    text: data.Message,
                });
            }
        },
        error: function (error) {

        }
    });
}