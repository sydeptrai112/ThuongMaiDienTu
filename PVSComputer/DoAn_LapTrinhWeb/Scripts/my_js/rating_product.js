function CRateOut(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
function CRateOver(rating) {
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
    }
}
function CRateClick(rating) {
    $('#ConfirmAdd').removeAttr('disabled');
    $('#FBk_Content').removeAttr('disabled');
    $("#dcript_content_fb").css("color", "#666");
    $("#dcript_content_fb").text("Nhập nội dung đánh giá");
    $("#lblRating").val(rating);
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');
       
    }
    for (var i = 1; i <= 5; i++) {
        $("#rate" + i).attr('class', 'fa fa-star-o');
    }
}
function CRateSelected() {
    var rating = $("#lblRating").val();
    for (var i = 1; i <= rating; i++) {
        $("#rate" + i).attr('class', 'fa fa-star');

    }
}

//phải login trước khi bình luận
$(".rating_login").click(function () {
    var self = $(this);
    console.log(self.data('title'));
    $.get("/account/userlogged", {},
        function (isLogged, textStatus, jqXHR) {
            if (!isLogged) {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 2000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'yêu cầu đăng nhập?'
                })
            }
            else {
                location.href = ev.currentTarget.href;
            }
        },
        "json"
    );
});
$('#uploadimgrate').change(function () {
    $(".btn_uploadimgrating").text("Đổi ảnh");
});
//preview ảnh
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('.preimg_rate')
                .attr('src', e.target.result);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
//đếm text
$('textarea').keyup(function () {

    var characterCount = $(this).val().length,
        current = $('#current'),
        maximum = $('#maximum'),
        theCount = $('#the-count');

    current.text(characterCount);


    /*This isn't entirely necessary, just playin around*/
    if (characterCount < 70) {
        $("#current").css("color", "#666");
        $("#dcript_content_fb").css("color", "#666");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }
    if (characterCount >= 70 && characterCount <= 150) {
        $("#current").css("color", "#47c90e");
        $("#dcript_content_fb").css("color", "#47c90e");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }
    if (characterCount > 150 && characterCount < 199) {
        $("#current").css("color", "#ff9900");
        $("#dcript_content_fb").css("color", "#ff9900");
        $("#dcript_content_fb").text("Nội dung tối thiểu 70 ký tự");
    }

    if (characterCount >= 200) {
        $("#maximum").css("color", "#8f0001");
        $("#current").css("color", "#8f0001");
        $("#dcript_content_fb").css("color", "#8f0001");
        $("#current").css("font-family", "Roboto-Medium");
        $("#maximum").css("font-family", "Roboto-Medium"); 
        $("#dcript_content_fb").text("Nội dung đã đạt đến giới hạn");
    } else {
        $("#maximum").css("color", "#666");
    }


});

//xử lý lưu dữ liệu database
function AjaxPost(formData) {
    $.ajax({
        type: 'post',
        url: '/Products/RatingProduct',
        data: new FormData(formData),
        contentType: false,
        processData: false,
        success: function (result) {
        }
    })
}