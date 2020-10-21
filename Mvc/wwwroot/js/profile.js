$(document).ready(function () {

    firebase.auth().onAuthStateChanged(function (user) { // NAV BAR
        if (user) { //giris yapıldıysa
            $("#checkBtn").click(function () {
                alert(user.emailVerified);

            });
            $("#checkNameBtn").click(function () {
                alert(user.displayName);

            });
            if (user.emailVerified == false) {
                $(".emailVerifiedoff").show();

                $("#verifyBtn").click(function () {
                    if (user.emailVerified == true) {
                        alert("12");
                        window.location.href = "#";
                    }
                    user.sendEmailVerification().then(function () {
                        alert("Doğrulma epostası gönderildi lütfen epostanızı kontrol edin.");
                    }).catch(function (error) {
                        alert("Doğrulma epostası gönderilemedi.\n Lütfen tekrar deneyiniz.");
                    });
                });
            } else {
                $(".emailVerifiedon").show();
            }
        } else {
            window.location.href = "/home";
        }
    });


});
