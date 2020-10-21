$(document).ready(function () {

    //var config = {
    //    apiKey: "AIzaSyAtTXK87wUiYGeCjqmGKqlGzKRJ7DgC2nE",
    //    authDomain: "viking-cf576.firebaseapp.com",
    //    databaseURL: "https://viking-cf576.firebaseio.com",
    //    projectId: "viking-cf576",
    //    storageBucket: "viking-cf576.appspot.com",
    //    messagingSenderId: "832727375608",
    //    appId: "1:832727375608:web:c61e14a31548046e0a250d",
    //    measurementId: "G-FW0GR31S8J"
    //};
    //firebase.initializeApp(config);

    //var email = $("#email").val();
    //var password = $("#password").val();

    //firebase.auth().signInWithEmailAndPassword(email, password)
    //    .then(function () {
    //        window.location.href = "/home";
    //        //kullanıcı adını kaydet
    //    }).catch(function (error) {
    //        alert(error.message);
    //    })

    firebase.auth().onAuthStateChanged(function (user) {
        if (user) { //giris yapıldıysa

        }
    });

});
