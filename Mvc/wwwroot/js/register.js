$(document).ready(function () {
    var ip;
    $.getJSON('https://ipapi.co/json/',
        function (data) {
            ip = JSON.stringify(data.ip, null, 2);
        });

    $("#registerBtn").click(function () {

        var email = $("#email").val();
        var nickname = $("#nickname").val();
        var password = $("#password").val();

        var db = firebase.firestore();
        var usersRef = db.collection('user');
        usersRef.where('k_adi', '==', nickname).get()
            .then(snapshot => {
                if (snapshot.empty) {
                    //return firebase.auth().createUserWithEmailAndPassword(this.state.email, this.state.password);
                    //kullanıcı adı kullanılmıyor
                    firebase.auth().createUserWithEmailAndPassword(email, password).then(function (credential) {
                        db.doc("user/" + credential.user.uid).set(
                            {
                                biyografi: "",
                                eposta: credential.user.email,
                                g_ad: "",
                                g_soyad: "",
                                ip: ip,
                                k_adi: nickname,
                                pp_link: "firebasestorage.googleapis.com/v0/b/viking-cf576.appspot.com/o/pp_default.jpg?alt=media&token=7f0ef854-493d-4a49-a15f-8e7a59008c6c",
                                sehir: "",
                                uid: credential.user.uid
                            }
                        ).then(function () {
                            console.log("ok reg");
                            window.location.href = "/home";
                        }).catch(function (error) {
                            alert("1 :" + error.message);
                        });

                        firebase.auth().signInWithEmailAndPassword(email, password)
                            .then(function () {
                                window.location.href = "/home";
                                //kullanıcı adını kaydet
                            }).catch(function (error) {
                                alert("2 :" + error.message);
                            })

                    });

                } else {
                    //kullanıcı adı alınmıs
                    alert("Haydaaa. Bu kullanıcı adı zten alınmıs.");
                    throw new Error('username already taken');
                }
            }).catch(err => {
                console.log('Error: ', err);
            });

    });
});



//firebase.auth().createUserWithEmailAndPassword(email, password)
//    .then(function (credential) {


//    }).catch(function (error) {
//        alert("3 :" + error.message);
//    });



//firebase.auth().signInWithEmailAndPassword(email, password)
//    .then(function () {
//        alert(credential.user.uid);

//        firebase.firestore().doc("user/" + credential.user.uid).set(
//            {
//                biyografi: "",
//                eposta: credential.user.email,
//                g_ad: "",
//                g_soyad: "",
//                ip: ip,
//                k_adi: nickname,
//                pp_link: "firebasestorage.googleapis.com/v0/b/viking-cf576.appspot.com/o/pp_default.jpg?alt=media&token=7f0ef854-493d-4a49-a15f-8e7a59008c6c",
//                sehir: "",
//                uid: credential.user.uid
//            }
//        ).then(function () {
//            console.log("ok reg");
//        }).catch(function (error) {
//            alert("1 :" + error.message);
//        });

//        window.location.href = "/home";
//        //kullanıcı adını kaydet
//    }).catch(function (error) {
//        alert("2 :" + error.message);
//    });
