/*
 * Format giỏ hàng: product_{id}={quantity}
 */
// Sự kiện click [Thêm vào giỏ] ở trang detail
$("#btnAddToCart").click(function (ev) {
	var exdays = 2;		// Sản phẩm trong giỏ hàng sẽ tự động xóa sau 2 ngày
	var id = $(ev.currentTarget).data("id");
	//thêm số lượng dựa theo input đầu vào
	var quan = $(ev.currentTarget).prev().find("input").val();
	var cookieName = "product_" + id;
	var productInCart = Cookie.get(cookieName);
	if (productInCart) {
		// Có sản phẩm trong giỏ hàng
		quan = Number(productInCart) + Number(quan);
	}
	Cookie.set(cookieName, quan, exdays);
	var cartCountUI = $("#lblCartCount");
	if (cartCountUI.length) {
		cartCountUI.text(Cookie.countWithPrefix("product"));
	} else {
		$("#lblCartCount").text(1);
	}
	// Hiển thị thông báo
	const Toast = Swal.mixin({
		toast: true,
		position: 'top',
		showConfirmButton: false,
		timer: 1000,
		timerProgressBar: false,
		didOpen: (toast) => {
			toast.addEventListener('mouseenter', Swal.stopTimer)
			toast.addEventListener('mouseleave', Swal.resumeTimer)
		}
	})
	Toast.fire({
		icon: 'success',
		title: 'Thêm sản phẩm thành công'
	})
});
