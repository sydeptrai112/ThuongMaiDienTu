$(window).on('load', function () {
    const mgs_type = $('input[name=mgs_type]').val();
    const msg = $('input[name=mgs]').val();
    if (mgs_type != '' && msg != '') {
        toastr.options = {
            "debug": false,
            "newestOnTop": true,
            "positionClass": "toast-top-center",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "250",
            "hideDuration": "800",
            "timeOut": "3000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        };
        switch (mgs_type) {
            case 'success':
                toastr.success(msg);
                break;
            case 'info':
                toastr.info(msg);
                break;
            case 'warning':
                toastr.warning(msg);
                break;
            case 'danger':
                toastr.error(msg);
                break;
        }
    }
});

// 404 không gọi được
// 500 lời gọi sai ajax
