let userForm = document.querySelector('#user-clear-search-form');
let userClearAnchor = documen.querySelector('#user-clear-anchor');

userClearAnchor.addEventListener('click', function(e) {
    let userSearchForm = document.querySelector('#user-search-form');
    userSearchForm.reset();
})