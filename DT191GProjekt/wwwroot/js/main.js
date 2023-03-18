//Mobile navigation (hamburger menu) on small screen

function hamburgerNav() {
    var x = document.getElementById("topnav");
    if (x.className === "topmenu") {
        x.className += " responsive";
    } else {
        x.className = "topmenu";
    }
}