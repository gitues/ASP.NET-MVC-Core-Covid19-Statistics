function PleaseWait() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
}

function CloseWait() {
    $.unblockUI();
}

function doExport(params) {
    var options = {
        tableName: 'ToptenCovid19Cases',
        fileName: 'ToptenCovid19Cases_file',
        worksheetName: 'TopTen'
    };

    $.extend(true, options, params);
    $('.table').tableExport(options);
}


