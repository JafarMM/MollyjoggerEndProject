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



function pluss() {
    var count = Number(document.getElementById('count').innerHTML);
    var price = Number(document.getElementById('hideprice').innerHTML);
    count++;
    var total = count * price;
    document.getElementById('count').innerHTML = count;
    document.getElementById('total').innerHTML = "Total: "+total+ " USD";
}

function minuss() {
    var count = Number(document.getElementById('count').innerHTML);
    var price = Number(document.getElementById('hideprice').innerHTML);
    if (count > 0) {
        count--;
    }
    var total = count * price;
    document.getElementById('count').innerHTML = count;
    document.getElementById('total').innerHTML = "Total: " + total + " USD";
}