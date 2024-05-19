$(".select2").select2({
    placeholder: "Select",
    allowClear: true
});

$('#BankNameId').on('select2:select', function (event) {
    var selectedBankId = event.params.data.id;
    $.ajax({
        type: 'POST',
        url: '/ATMLocation/BankBranchSelectItemsByBankNameId',
        data: { bankNameId: selectedBankId },
        dataType: 'json',
        success: function (data) {
            $("#BankBranchNameId").prop("disabled", false);
            $('#BankBranchNameId option').remove();
            var bankBranchDDL = $('#BankBranchNameId');
            bankBranchDDL.append(`<option value="">Select</option>`);

            data.forEach(function (bankBranch) {
                bankBranchDDL.append(new Option(bankBranch.Text, bankBranch.Value, false, false));
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
        url: '/ATMLocation/TownshipSelectItemsByDivisionId',
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

    if ($('#editATMLocationForm').valid()) {
        // Show loading dialog
        const loadingDialog = Swal.fire({
            title: 'Loading...',
            allowOutsideClick: false,
            showConfirmButton: false, // Hide OK button
            willOpen: () => {
                Swal.showLoading();
            },
        });

        var data = $("#editATMLocationForm").serialize();

        $.ajax({
            type: 'POST',
            url: '/ATMLocation/Update/' + $("#ATMLocationId").val(),
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