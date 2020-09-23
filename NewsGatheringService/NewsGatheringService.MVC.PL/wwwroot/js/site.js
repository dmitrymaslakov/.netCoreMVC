$('#all').click(function () {
    $(this).closest('table').find('td input:checkbox').prop('checked', this.checked);
});


$('#js-load').click(function () {
    let _this = $(this);
    let y = $('table td :checkbox:checked').map(function () {
        return $(this).closest('tr').find('a[href]').first().attr('href');
    }).get();

    $.ajax({
        url: '/Admin/AddNewsToDb',
        type: "post",
        data: { newsUrls: y },
        beforeSend: function () {
            _this
                .prop('disabled', true)
                .find('.spinner-grow').removeClass('d-none');
        },
    })
        .done(function () {
            $('table td :checkbox:checked').each(function () {
                $(this).closest('tr').remove();
            });
            _this
                .prop('disabled', false)
                .find('.spinner-grow').addClass('d-none');
            alert('Загрузка завершена.');
        })
        .fail(function () {
            alert("error");
        });
});