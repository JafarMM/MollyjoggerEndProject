
$(document).ready(function () {

    // PRODUCT

    $(document).on('click', '.categories', function (e) {
        e.preventDefault();
        $(this).next().next().slideToggle();
    });

    $(document).on('click', '.browseul li a', function (e) {
        e.preventDefault();
        let browseul = $(this).attr('data-id');
        let products = $('.product-item');

        products.each(function () {
            if (category == $(this).attr('data-id')) {
                $(this).parent().fadeIn();
            }
            else {
                $(this).parent().hide();
            }
        })
        if (category == 'all') {
            products.parent().fadeIn();
        }
    });




    $(document).on('click', '.tab4 ul li', function () {
        $(this).siblings('.active').removeClass('active');
        $(this).addClass('active');
        let dataId = $(this).attr('data-id');
        $(this).parent().parent().next().children().children('p.active').removeClass('active');

        $(this).parent().parent().next().children().children('p').each(function () {
            if (dataId == $(this).attr('data-id')) {
                $(this).addClass('active')
            }
        });
    });



    let skip = 6;
    $(document).on('click', '#loadMore', function () {
        $.ajax({
            url: '/Shop/Load?skip=' + skip,
            type: 'GET',
            success: function (res) {


                $("#productList").append(res);
                skip += 6;

                let productsCount = $("#productsCount").val();

                if (skip >= productsCount) {
                    $("#loadMore").remove();
                }
            }
        });
    });

});


var navLinks = document.getElementById("navLinks");

function showMenu() {
    navLinks.style.right = "0%";
}

function hideMenu() {
    navLinks.style.right = "-100%";
}
