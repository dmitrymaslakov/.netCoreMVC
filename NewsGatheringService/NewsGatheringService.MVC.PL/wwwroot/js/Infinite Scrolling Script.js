$(function () {

    $('div#loading').hide();

    let categoryName = $('#block1').attr('data-category');
    let subcategoryName = $('#block1').attr('data-subcategory');
    let rateValue = $('#block1').attr('data-rate');

    let page = 1;
    let _inCallback = false;
    function loadItems() {
        if (page > -1 && !_inCallback) {
            _inCallback = true;
            page++;
            $('div#loading').show();

            $.ajax({
                type: 'GET',
                url: '/Home/Index',
                data: {
                    'page': page,
                    'categoryname': categoryName,
                    'subcategoryname': subcategoryName,
                    'rate': rateValue
                },
            success: function (data, textstatus) {
                    if (data != '') {
                        $("#scrolList").append(data);
                    }
                    else {
                        page = -1;
                    }
                    _inCallback = false;
                    $("div#loading").hide();
                }
            });
        }
    }
    // обработка события скроллинга
    $(window).scroll(function () {
        if ($(window).scrollTop() == $(document).height() - $(window).height()) {

            loadItems();
        }
    });
})
/*let pageSize = +$('#block1').attr('data-psize');
let pageIndex = 2;
let pages = +$('#block1').attr('data-ps');
let categoryName = $('#block1').attr('data-category');
let subcategoryName = $('#block1').attr('data-subcategory');
let _incallback = false;
//let rateValue = $('#ex7').slider('getValue');


$(window).scroll(function () {
    let hT = $('#progressmarker').offset().top;
    let hH = $('#progressmarker').outerHeight();
    let wH = $(window).height();
    let wS = $(window).scrollTop();
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
            'page': pageIndex,
            'categoryname': categoryName,
            'subcategoryname': subcategoryName,
            //'rate': rateValue
        },
        dataType: 'html',
        success: function (data) {
            if (data != null) {
                $('#IndexItemsPart').append(data);
                pageIndex++;
            }
        },
        beforeSend: function () {
            $('#progress').show();
        },
        complete: function () {
            $('#progress').hide();
            _incallback = false;
        },
        error: function () {
            _incallback = false;
            alert('Error while retrieving data!');
        }
    });
}*/
