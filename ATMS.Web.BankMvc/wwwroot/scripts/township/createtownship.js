$(".select2").select2({
    placeholder: "Select",
    allowClear: true
});

$('#RegionId').on('select2:select', function (event) {
    var selectedRegionId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/Township/DivisionSelectItemsByRegionId',
        data: { regionId: selectedRegionId },
        dataType: 'json',
        success: function (data) {
            data.forEach(function (division) {
                $('#DivisionId').append(new Option(division.Text, division.Value, false, false));
            });
        },
        error: function () {

        }
    });
});

$('.saveBtn').on('click', function (event) {
    event.preventDefault();

    if ($('#createTownshipForm').valid()) {
        // Show loading dialog
        const loadingDialog = Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showConfirmButton: false, // Hide OK button
            willOpen: () => {
                Swal.showLoading();
            },
        });

        var data = $("#createTownshipForm").serialize();

        $.ajax({
            type: 'POST',
            url: '/Township/Save',
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
    else {
        return false;
    }
});