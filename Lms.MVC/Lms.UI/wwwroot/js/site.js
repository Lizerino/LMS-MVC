let userForm = document.querySelector('.clear-search-form');
let userClearAnchor = document.querySelector('.clear-anchor');

userClearAnchor.addEventListener('click', function (e) {
    let userForm = document.querySelector('.clear-search-form');
    userForm.submit();
})

userForm.addEventListener('submit', function (e) {
    let userSearchForm = document.querySelector('.search-form');
    userSearchForm.reset();
})