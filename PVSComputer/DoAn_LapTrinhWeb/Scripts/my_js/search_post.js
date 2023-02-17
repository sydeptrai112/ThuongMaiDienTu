$(document).ready(function () {
    $("#news_title").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/News/GetPostSearch",
                type: "POST",
                dataType: "json",
                data: { Prefix: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.news_title, value: item.news_title, slug: item.slug, image: item.image};
                    }))
                }
            })
        },
        create: function (event, ui) {
            $(this).data('ui-autocomplete')._renderItem = function (ul, item) {
                return $('<li>')
                    .append("<img src=" + item.image + " alt='img' class='img_search'/>")
                    .append('<span class="lable_searchpost">' + item.label + '</span>')
                    .appendTo(ul);
            };
        },
        minLength: 0,
        messages: {
            noResults: "Không tìm thấy bài viết"
            ,
            results: function (count) {
                return count + (count > 1 ? ' results' : ' result ') + ' found';
            }
        },
        select: function (event, ui) {
            window.location.href = '/tin-tuc/' + ui.item.slug;
            return false;
        }
    })

})