let pageSize = +$('#block1').attr('data-psize');
let pageIndex = 2;
let pages = +$('#block1').attr('data-ps');
let categoryName = $('#block1').attr('data-category');
let subcategoryName = $('#block1').attr('data-subcategory');
let _incallback = false;

$(window).scroll(function () {
    let hT = $('#progressmarker').offset().top;
    let hH = $('#progressmarker').outerHeight();
    let wH = $(window).height();
    let wS = $(window).scrollTop() + 3;
    if (pages >= pageIndex && !_incallback) {
        let merg = hT + hH - wH;
        if (wS > merg) {
            GetData();
        }
    }
});

function GetData() {
    _incallback = true;
    $.ajax({
        type: 'GET',
        url: '/Home/IndexItemsPart',
        data: {
            "page": pageIndex,
            "categoryname": categoryName,
            "subcategoryname": subcategoryName
        },
        dataType: 'html',
        success: function (data) {
            if (data != null) {
                $("#IndexItemsPart").append(data);
                pageIndex++;
            }
        },
        beforeSend: function () {
            $("#progress").show();
        },
        complete: function () {
            $("#progress").hide();
            _incallback = false;
        },
        error: function () {
            _incallback = false;
            alert("Error while retrieving data!");
        }
    });
}
