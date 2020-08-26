//let set = new Set();

//$(document).ready(function () {
//    let table = $('.table.table-checks');
//    table
//        .on('change', '#all', function () {
//            $('> tbody input:checkbox', table).prop('checked', $(this).is(':checked')).trigger('change');
//        });

//    table.on('change', function () {
//        $('> tbody input:checkbox', table).each(function () {
//            if ($(this).is(':checked')) {
//                set.add($(this).parents("tr").find('.source').text());
//            }
//        });
//    });
//});
$('#all').click(function () {
    $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
});


$('.btn-primary').click(function () {
    let y = $('table td :checkbox:checked').map(function () {
        return $(this).closest('tr').find('td.source').first().text();
    }).get();

    $.post('/Admin/AddNewsToDb', { newsIds: y }, function (data) {
    })
        .done(function () {
            let q = $('table td :checkbox:checked').each(function () {
                $(this).closest('tr').fadeOut("slow");
            });
        })
        .fail(function () {
            alert("error");
        });
    //$.ajax({
    //    url: 'Home/Data',
    //    type: "post",
    //    data: { data: y },
    //    success: function (result) {
    //        console.log(result);
    //    }
    //});

});