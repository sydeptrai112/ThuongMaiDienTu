/*
 * Format giỏ hàng: product_{id}={quantity}
 */
// Khởi tạo giỏ hàng khi vào trang giỏ hàng
$(window).ready(function (ev) {
	$("#cartCount").text(Cookie.countWithPrefix("product"));
});
// Button xóa sản phẩm khỏi giỏ hàng
$(".js-delete").click(function (ev) {
	bootbox.confirm({
		message: "Xoá sản phẩm?",
		buttons: {
			confirm: {
				label: 'Xoá',
				className: 'btn-danger'
			},
			cancel: {
				label: 'Quay lại',
				className: 'btn-secondary'
			}
		},
		callback: function (result) {
			if (result) {
				var id = $(ev.currentTarget).data("id");
				var item = $(ev.currentTarget).closest(".item");
				item.remove();
				Cookie.remove("product_" + id);
				var productCount = Cookie.countWithPrefix("product")
				$("#cartCount").text(productCount);
				$("#lblCartCount").text(productCount == 0 ? "" : productCount);
				updateOrderPrice();
				setTimeout(function () {
					window.location.reload();
				}, 1000);
			}
		}
	})
});
//
//cập nhật giỏ hảng
function updateOrderPrice() {
	var quanInputs = $("input.qty-input");
	var newTotal = 0;
	quanInputs.each(function (i, e) {
		var price = Number($(e).data('price'));
		var quan = Number($(e).val());
		newTotal += price * quan;
	});
	var eleDiscount = $("#discount");
	var discount = 0;
	if (eleDiscount.attr("data-price")) {
		discount = Number(eleDiscount.attr("data-price"));
	}
	var totalWithFee = newTotal + 30000 - discount;
	totalWithFee += "";
	newTotal += "";
	discount += "";
	var regex = /(\d)(?=(\d{3})+(?!\d))/g;
	$("#totalPrice").text(newTotal.replace(regex, "$1,") + "₫");
	$("#totalPriceWithFee").text(totalWithFee.replace(regex, "$1,") + "₫");
	$("#discount").text(discount.replace(regex, "$1,") + "₫");
};
//được sử dụng khi bấm nút thanh toán
$(".js-checkout").click(function (ev) {
	ev.preventDefault();
	var self = $(this);
	console.log(self.data('title'));
	$.get("/account/userlogged", {},
		function (isLogged, textStatus, jqXHR) {
			if (!isLogged) {
				//gọi action đăng nhập khi người dùng bấm thanh toán mà chưa đăng nhập hệ thống
				bootbox.confirm({
					message: "!Vui lòng đăng nhập để thực hiện chức năng thanh toán",
					buttons: {
						confirm: {
							label: 'Đăng nhập',
							className: 'btn-info'
						},
						cancel: {
							label: 'Quay lại',
							className: 'btn-secondary'
						}
					},
					callback: function (result) {
						if (result) {
							window.location = "/Account/SignIn";
						}
					}
				});
			}
			else {
				location.href = ev.currentTarget.href;
			}
		},
		"json"
	);
});
//Sử dụng code giảm giá
$(".btn-submitcoupon").click(function (ev) {
	var input = $(ev.currentTarget).prev();
	var _code = input.val().trim();
	if (_code.length) {
		$.getJSON("/cart/UseDiscountCode", { code: _code },
			function (data, textStatus, jqXHR) {
				if (data.success) {
					$("#discount").attr("data-price", data.discountPrice);
					updateOrderPrice();
					// Hiển thị thông báo
					const Toast = Swal.mixin({
						toast: true,
						position: 'top',
						showConfirmButton: false,
						timer: 1500,
						timerProgressBar: true,
						didOpen: (toast) => {
							toast.addEventListener('mouseenter', Swal.stopTimer)
							toast.addEventListener('mouseleave', Swal.resumeTimer)
						}
					})
					Toast.fire({
						icon: 'success',
						title: 'Đã áp dụng mã giảm giá'
					})
				} else {
					const Toast = Swal.mixin({
						toast: true,
						position: 'top',
						showConfirmButton: false,
						timer: 1500,
						timerProgressBar: true,
						didOpen: (toast) => {
							toast.addEventListener('mouseenter', Swal.stopTimer)
							toast.addEventListener('mouseleave', Swal.resumeTimer)
						}
					})
					Toast.fire({
						icon: 'warning',
						title: 'Không thể sử dụng mã giảm giá này, vui lòng kiểm tra lại'
					})
				}
			}
		);
	}
})


//var QtyInput = (function () {
//	var $qtyInputs = $(".quantity_cart");
//	if (!$qtyInputs.length) {
//		return;
//	}
//	var $inputs = $qtyInputs.find(".qty-input");
//	var $countBtn = $qtyInputs.find(".value-button");
//	var qtyMin = parseInt($inputs.attr("min"));
//	var qtyMax = parseInt($inputs.attr("max"));

//	$inputs.change(function () {
//		var $this = $(this);
//		var $minusBtn = $this.siblings(".btn-dec");
//		var $addBtn = $this.siblings(".btn-inc");
//		var qty = parseInt($this.val());

//		if (isNaN(qty) || qty <= qtyMin) {
//			$this.val(qtyMin);
//			$minusBtn.attr("disabled", true);
//		} else {
//			$minusBtn.attr("disabled", false);

//			if (qty <= qtyMax) {
//				$this.val(qtyMax);
//				$addBtn.attr('disabled', true);
//			} else {
//				$this.val(qty);
//				$addBtn.attr('disabled', false);
//			}
//		}
//	});

//	$countBtn.click(function (ev) {
//		var operator = this.dataset.action;
//		var $this = $(this);
//		var $input = $this.siblings(".qty-input");
//		var qty = parseInt($input.val());
//		if (operator == "add") {
//			var quanInput = $(ev.currentTarget).prev();
//			var id = quanInput.data("id");
//			var quan = Number(quanInput.val());
//			quan = quan + 1;
//			qty += 1;
//            Cookie.set("product_" + id, quan, 2);
//            quanInput.val(quan);
//			updateOrderPrice();
//			if (qty >= qtyMin + 1) {
//				$this.siblings(".btn-dec").attr("disabled", false);
//			}
//			if (qty >= qtyMax) {
//				$this.attr("disabled", true);
//			}
//		} else {
//			var quanInput = $(ev.currentTarget).next();
//			var id = quanInput.data("id");
//			var quan = Number(quanInput.val());
//			qty -= 1;
//			quan = quan - 1;
//			Cookie.set("product_" + id, quan, 2);
//			quanInput.val(quan);
//			updateOrderPrice();
//			if (qty == qtyMin) {
//				$this.attr("disabled", true);
//			}

//			if (qty < qtyMax) {
//				$this.siblings(".btn-inc").attr("disabled", false);
//			}
//		}
//		$input.val(qty);
//	});
//})();


// Button giảm số lượng
$(".btn-dec").click(function (ev) {
	var quanInput = $(ev.currentTarget).next();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	if (quan > 1) {
		quan = quan - 1;
		Cookie.set("product_" + id, quan, 2);
		quanInput.val(quan);
		updateOrderPrice();
	}
});
// Button tăng số lượng
$(".btn-inc").click(function (ev) {
	var quanInput = $(ev.currentTarget).prev();
	var id = quanInput.data("id");
	var quan = Number(quanInput.val());
	if (quan < 1) {
		quan = 1;
		Cookie.set("product_" + id, quan, 2);
		quanInput.val(quan);
		updateOrderPrice();
	}
	else {
		quan = quan + 1;
		Cookie.set("product_" + id, quan, 2);
		quanInput.val(quan);
		updateOrderPrice();
    }
});