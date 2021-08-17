 



var navLinks=document.getElementById("navLinks");
var browseul=document.getElementById("browseul");
var HideCategory=document.getElementById("HideCategory")
var searchDown=document.getElementById("searchDown")
var searchMain=document.getElementById("searchMain")
function showMenu(){
    navLinks.style.right="0%";
}

function hideMenu(){
    navLinks.style.right="-100%";
}

function showCategories(){
    browseul.style.display="block"
    HideCategory.style.display="block"
}
function hideCategories(){
    browseul.style.display="none"
    HideCategory.style.display="none"
}

function showSearch(){
    searchDown.style.display="block"
}
function hideSearch(){
    searchDown.style.display="none"
}


var MainImg=document.getElementById("MainImg");
var smallimg=document.getElementsByClassName('small-img');

smallimg[0].onclick=function(){
    MainImg.src=smallimg[0].src;
}
smallimg[1].onclick=function(){
    MainImg.src=smallimg[1].src;
}
smallimg[2].onclick=function(){
    MainImg.src=smallimg[2].src;
}
smallimg[3].onclick=function(){
    MainImg.src=smallimg[3].src;
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