let subInput;
$(document).on("click", "#button-subscribe", function () {
    $("#response-subscribe").empty()
    subInput = $("#Email-subscribe").val()
    if (subInput.length > 1) {
        $.ajax({
            type: "Get",
            url: "Home/Subscribe",
            data: {
                "email": subInput
            },
            success: function (res) {
                $("#response-subscribe").append(res)
            }
        })
    }

})


let tests = document.querySelectorAll(".test1");
let tests2 = document.querySelectorAll(".test2");
let total = document.getElementById("total");
let pr = document.querySelectorAll("#price");

pr.forEach(item => {
    total.innerHTML = Number(total.innerHTML) + Number(item.innerHTML.split(": ")[1]);
})

tests.forEach(item => {
    item.addEventListener("click", function () {
        console.log(total.innerHTML);
        let count = Number(this.nextElementSibling.innerHTML)
        count++;
        this.nextElementSibling.innerHTML = count;
        let td = this.parentElement
        let price = td.previousElementSibling.innerHTML;
        let priceValue = Number(price.split(": ")[1]);
        console.log(priceValue);
        console.log(count);
        //total.innerHTML = `Total: ${priceValue * count}`;
        pr.forEach(item => {
            total.innerHTML = Number(priceValue * count) + Number(item.innerHTML.split(": ")[1]);
        })
        
    });
})
tests2.forEach(item => {
    item.addEventListener("click", function () {
        let count = Number(this.previousElementSibling.innerHTML)
        count--;
        this.previousElementSibling.innerHTML = count;
        let td = this.parentElement
        let price = td.previousElementSibling.innerHTML;
        let priceValue = Number(price.split(": ")[1]);
        //total.innerHTML = `Total: ${priceValue * count}`;

        total.innerHTML = Number(total.innerHTML) - Number(priceValue * count);
    });
})

$(document).on("click", ".category-item", function () {
    $("#productList").empty();
    if ($(this).hasClass("active-category")) {
        $(this).removeClass("active-category");
    } else {
        $(this).addClass("active-category");
    }

    $.ajax({
        type: "Get",
        url: "Shop/Filter",
        data: {
            id: $(this).attr("id")
        },
        success: function (res) {
            $("#productList").append(res);
        }
    })
})
