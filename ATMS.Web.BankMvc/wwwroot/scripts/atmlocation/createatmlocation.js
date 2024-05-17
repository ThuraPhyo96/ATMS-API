$(".select2").select2({
    placeholder: "Select",
    allowClear: true
});

$('#BankNameId').on('select2:select', function (event) {
    var selectedBankNameId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/ATMLocation/BankBranchSelectItemsByBankNameId',
        data: { bankNameId: selectedBankNameId },
        dataType: 'json',
        success: function (data) {
            data.forEach(function (bankbranches) {
                $('#BankBranchNameId').append(new Option(bankbranches.Text, bankbranches.Value, false, false));
            });
        },
        error: function () {

        }
    });
});

$('#RegionId').on('select2:select', function (event) {
    var selectedRegionId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/ATMLocation/DivisionSelectItemsByRegionId',
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

$('#DivisionId').on('select2:select', function (event) {
    var selectedDivisionId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/ATMLocation/TownshipSelectItemsByDivisionId',
        data: { divisionId: selectedDivisionId },
        dataType: 'json',
        success: function (data) {
            data.forEach(function (township) {
                $('#TownshipId').append(new Option(township.Text, township.Value, false, false));
            });
        },
        error: function () {

        }
    });
});

$('.saveBtn').on('click', function (event) {
    event.preventDefault();

    if ($('#atmLocationForm').valid()) {
        // Show loading dialog
        const loadingDialog = Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showConfirmButton: false, // Hide OK button
            willOpen: () => {
                Swal.showLoading();
            },
        });

        var data = $("#atmLocationForm").serialize();

        $.ajax({
            type: 'POST',
            url: '/ATMLocation/Save',
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
    else {
        return false;
    }
});