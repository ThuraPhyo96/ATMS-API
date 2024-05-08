//#region
var options = {
    series: _bankSeries,
    chart: {
        width: 380,
        type: 'pie',
    },
    labels: _bankLabels,
    responsive: [{
        breakpoint: 480,
        options: {
            chart: {
                width: 200
            },
            legend: {
                position: 'bottom'
            }
        }
    }]
};

var chart = new ApexCharts(document.querySelector("#chart"), options);
chart.render();
//#endregion

//#region
var baroptions = {
    series: [{
        data: _bankSeries
    }],
    chart: {
        type: 'bar',
        height: 350
    },
    plotOptions: {
        bar: {
            borderRadius: 4,
            borderRadiusApplication: 'end',
            horizontal: true,
        }
    },
    dataLabels: {
        enabled: false
    },
    xaxis: {
        categories: _bankLabels,
    }
};

var barchart = new ApexCharts(document.querySelector("#bar"), baroptions);
barchart.render();
//#endregion