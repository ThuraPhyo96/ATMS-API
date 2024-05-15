function loadPage(pagenumber, pagesize) {
    $.ajax({
        type: 'POST',
        url: '/BankBranchAjax/PartialPagination/',
        contentType: 'application/x-www-form-urlencoded',
        data:
        {
            pageNo: pagenumber,
            pageSize: pagesize
        },
        success: function (data) {
            $("#grid").html(data);
        },
        error: function (error) {

        }
    });
}