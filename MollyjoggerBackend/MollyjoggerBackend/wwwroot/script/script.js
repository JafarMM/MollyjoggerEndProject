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
        let id = $(".active-category").attr("id");
        if (id === undefined) {
            id = "0"
        }
        console.log(id);
        $.ajax({
            url: '/Shop/Load',
            data: {
                skip: skip,
                id: id
            },
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
var browseul = document.getElementById("browseul");
var HideCategory = document.getElementById("HideCategory")
var searchDown = document.getElementById("searchDown")
var searchMain = document.getElementById("searchMain")
 

function showMenu() {
    navLinks.style.right = "0%";
}

function hideMenu() {
    navLinks.style.right = "-100%";
}

function showCategories() {
    browseul.style.display = "block"
    HideCategory.style.display = "block"
}
 
function hideCategories() {
    browseul.style.display = "none"
    HideCategory.style.display = "none"
}


function showSearch() {
    searchDown.style.display = "block"
}
function hideSearch() {
    searchDown.style.display = "none"
}


let search;
$(document).on("keyup", "#home-input-search", function () {
    search = $(this).val().trim();

    $("#searchResult ul").remove();

    if (search.length > 0) {
        $.ajax({
            url: '/Home/Search?search=' + search,
            type: "GET",
            success: function (res) {
                $("#searchResult").append(res);
            }
        });
    }
});

var MainImg = document.getElementById("MainImg");
var smallimg = document.getElementsByClassName('small-img');

smallimg[0].onclick = function () {
    MainImg.src = smallimg[0].src;
}
smallimg[1].onclick = function () {
    MainImg.src = smallimg[1].src;
}
smallimg[2].onclick = function () {
    MainImg.src = smallimg[2].src;
}
smallimg[3].onclick = function () {
    MainImg.src = smallimg[3].src;
}

