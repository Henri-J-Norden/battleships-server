function getGuid() {
    var guid = "";
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i].trim().split("=");
        if (cookie[0] === "guid") {
            guid = cookie[1];
        }
    }

    if (guid === "") console.log("ERROR: cookie 'guid' not found!");

    return guid;
}