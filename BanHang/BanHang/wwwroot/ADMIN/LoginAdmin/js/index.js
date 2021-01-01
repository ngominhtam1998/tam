function checkAdminName(inputAdminName) {
    return inputAdminName.length > 4;
};
function checkPassword(inputPassword) {
    return inputPassword.length > 4;
}

$(document).ready(function () {
    $('#BtnLogin').click(function () {
        var val1 = $("#input1").val();
        var val2 = $("#input2").val();
        $(".erorrAdminName").html("");
        $(".erorrPassword").html("");
        if (val1 == "") {
            $(".erorrAdminName").html("Chua nhap tai khoan");
            return false;
        }
        if (val2 == "") {
            $(".erorrPassword").html("Chua nhap mat khau");
            return false;
        }
        if (!checkAdminName(val1)) {
            $('.erorrAdminName').html("Admin name must be at least 5 characters");
            return false;
        }
        else {
            $('.erorrAdminName').html("");
        }
        if (!checkPassword(val2)) {
            $('.erorrPassword').html("Password must be at least 5 characters");
            return false;
        }
        else {
            $('.erorrPassword').html("");
        }
        $.ajax({
            url: '/Admin/Admin/Login',
            type: 'POST',

            data: {
                a: val1,
                b: val2
            },
            success: (content) => {
                console.log(content);
                var obj = JSON.parse(JSON.stringify(content));
                console.log(content);
                if (obj.IsSuccess == true) {
                    window.location.href = "https://localhost:44395/Admin/Admin/Index";
                }
                else {
                    $(".erorrAdminName").html(obj.MesTring);
                }

            }
        });
    });


});
