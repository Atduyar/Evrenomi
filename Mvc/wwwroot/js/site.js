$(document).ready(function () {

    var config = {
        apiKey: "AIzaSyAtTXK87wUiYGeCjqmGKqlGzKRJ7DgC2nE",
        authDomain: "viking-cf576.firebaseapp.com",
        databaseURL: "https://viking-cf576.firebaseio.com",
        projectId: "viking-cf576",
        storageBucket: "viking-cf576.appspot.com",
        messagingSenderId: "832727375608",
        appId: "1:832727375608:web:c61e14a31548046e0a250d",
        measurementId: "G-FW0GR31S8J"
    };
    firebase.initializeApp(config);

    firebase.auth().onAuthStateChanged(function (user) {                  // NAV BAR
        if (user) { //giris yapıldıysa
            $(".joinon").show();

            $("#logout").click(function () {
                firebase.auth().signOut()
                    .then(function () {
                        console.log("ok singout " + user.uid);
                        window.location.href = "/home";
                    })
            });

            $("#profil").click(function () {
                window.location.href = "/Account/Profile";
            });

        } else {
            $(".joinoff").show();
        }
    });

});
