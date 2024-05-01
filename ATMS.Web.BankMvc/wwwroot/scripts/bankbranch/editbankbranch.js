$(".select2").select2({
    placeholder: "Select",
    allowClear: true
});

$('.timepicker').datetimepicker({
    datepicker: false, // Hide date picker
    format: 'g:i', // 12-hour format
    formatTime: 'g:i a', // Also set the time format
    step: 30, // Set the interval to 30 minutes
    showTimePicker: false
});

$('#RegionId').on('select2:select', function (event) {
    var selectedRegionId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/BankBranchAjax/DivisionSelectItemsByRegionId',
        data: { regionId: selectedRegionId },
        dataType: 'json',
        success: function (data) {
            $("#DivisionId").prop("disabled", false);
            $('#DivisionId option').remove();
            var divisionDDL = $('#DivisionId');
            divisionDDL.append(`<option value="">Select</option>`);

            data.forEach(function (division) {
                divisionDDL.append(new Option(division.Text, division.Value, false, false));
            });
        },
        error: function () {

        }
    });
});

$('#DivisionId').on('select2:select', function (event) {
    var selectedDivisionId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/BankBranchAjax/TownshipSelectItemsByDivisionId',
        data: { divisionId: selectedDivisionId },
        dataType: 'json',
        success: function (data) {
            $("#TownshipId").prop("disabled", false);
            $('#TownshipId option').remove();
            var townshipDDL = $('#TownshipId');
            townshipDDL.append(`<option value="">Select</option>`);

            data.forEach(function (township) {
                townshipDDL.append(new Option(township.Text, township.Value, false, false));
            });
        },
        error: function () {

        }
    });
});

$('.updateBtn').on('click', function (event) {
    event.preventDefault();

    if ($('#editBankBranchForm').valid()) {
        // Show loading dialog
        const loadingDialog = Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showConfirmButton: false, // Hide OK button
            willOpen: () => {
                Swal.showLoading();
            },
        });

        var data = $("#editBankBranchForm").serialize();

        $.ajax({
            type: 'POST',
            url: '/BankBranchAjax/Update/' + $("#BankBranchNameId").val(),
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
                            window.location.href = '/BankBranchAjax/Index';
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
                            window.location.href = '/BankBranchAjax/Index';
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