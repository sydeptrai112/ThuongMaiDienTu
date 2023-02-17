var amodal = $('#addModal');
var delmodal = $('#deleteModal');
var emodal = $('#editModal');
var idde;
var idfix;
var deleteConfirm = function (id, title) {
    delmodal.find('.title_stickypost').text(title);
    delmodal.modal('show');
    idde = id;
}
$('#deleteBtn').click(function () {
    delmodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/DeleteStickyPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idde }),
        dataType: "json",
        success: function (recData) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'success',
                title: recData.Message
            })
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1500);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không xóa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
});
$('#addHot').click(function () {
    $('#IDPost').val(null);
    amodal.modal('show');

});
$('#addBtn').click(function () {
    var valprio = $('#newPri').val();
    var prio = 100;
    var idadd = $('#IDPost').val();
    if (valprio != "") {
        prio = valprio;
    }
    amodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/CreateStickyPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idadd, priority: prio }),
        dataType: "json",
        success: function (recData) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'success',
                title: recData.Message
            })
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            }

        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'warning',
                title: '!Lỗi, thêm thất bại'
            })
        }
    });
    clearDat();

});
var editPost = function (id, prior, title) {

    emodal.find('.modal-title').text(title);
    $('#Txtpriority').val(prior);
    emodal.modal('show');
    idfix = id;
}
$('#editBtn').click(function () {
    var newpri = $('#Txtpriority').val();
    emodal.modal('hide');
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/EditStickyPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idfix, priority: newpri }),
        dataType: "json",
        success: function (recData) {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top',
                showConfirmButton: false,
                timer: 2500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'success',
                title: recData.Message
            })
            if (recData.reload != false) {
                setTimeout(function () {
                    window.location.reload();
                }, 1000);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/>Không sửa được<br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
    //$('#CateNameEdit').val(null);
});
$('#IDPost').change(function () {
    checkPost()
});
$('#btnCheck').click(function () {
    checkPost()
});
var idz;
var checkPost = function () {
    idz = $('#IDPost').val();
    $.ajax({
        type: "POST",
        url: '/NewsAdmin/checkPost',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ id: idz }),
        dataType: "json",
        success: function (recData) {
            if (recData.valid == true) {
                $('#postName').text(recData.Message);
                $('#postName').removeClass('text-danger');
                $('#postName').addClass('text-primary');
                $('#addBtn').removeAttr('disabled');
                $('#postName').removeClass('d-none');
                $('#postName').addClass('d-block');
            }
            else if (recData.valid == false) {
                $('#postName').text(recData.Message);
                $('#postName').removeClass('d-none');
                $('#postName').addClass('d-block');
                $('#postName').removeClass('text-primary');
                $('#postName').addClass('text-danger');
                $('#addBtn').prop("disabled", true);
            }

        },
        error: function () {
            var notify = $.notify('<strong>Lỗi</strong><br/><br />', {
                type: 'pastel-warning',
                allow_dismiss: false,
            });
        }
    });
}
var clearDat = function () {
    $('#IDPost').val(null);
    $('#postName').text("ID không hợp lệ");
    $('#postName').removeClass('text-primary');
    $('#postName').addClass('text-danger');
}
$('.cleardt').click(function () {
    clearDat();
});

