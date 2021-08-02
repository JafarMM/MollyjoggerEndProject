const hamburger = document.getElementById('hamburger');
const navUL = document.getElementById('nav-ul');

hamburger.addEventListener('click',() => {
    navUL.classList.toggle('show');
});


const browse = document.getElementById('browse');
const browseUL = document.getElementById('browseul');

    browse.addEventListener('click',() =>{
        browseUL.classList.toggle('show');
    });